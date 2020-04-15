<template>
    <div
        class="calendar has-border-right has-border-top has-border-left is-flex is-flex-column is-flex-grow-1 is-relative is-unselectable"
        ref="dayView"
        v-touch:swipe.right="onPreviousSwipe"
        v-touch:swipe.left="onNextSwipe"
    >
        <div
            class="has-h-80px is-flex is-flex-row has-border-bottom"
            v-for="hour in hours"
            :key="`${hour.hour}-${hour.period}`"
            :id="`hour-${hour.hour}-${hour.period}`"
        >
            <!-- Axis -->
            <div
                class="is-flex is-flex-row has-w-40px has-border-right is-justify-content-end has-padding-right-1"
            >
                <span class="is-size-6">{{ hour.hour }}</span>
                <span class="is-size-7 has-text-grey">{{ hour.period }}</span>
            </div>

            <!-- Hour -->
            <div :class="determineHourBackground(hour.raw)">
                <div
                    v-for="i in [0,1,2,3]"
                    :key="i"
                    class="interval has-h-20px"
                    @mousedown.prevent="onDragStart(hour.raw + i * 15)"
                    @mouseover.self.stop="onDragUpdate(hour.raw + i * 15)"
                >
                    <calendar-block
                        @mousedown.stop="onBlockDragStart"
                        @mouseup.stop="onBlockDragEnd"
                        v-if="getBlock(hour.raw + i * 15)"
                        :value="getBlock(hour.raw + i * 15)"
                    />
                </div>
            </div>
        </div>
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { getModule } from 'vuex-module-decorators';
import SettingsStore from '../../store/settings/settings-store';
import CalendarStore from '../../store/calendar/calendar-store';
import store from '../../../../core/store';
import { HoursOfOperationDay } from '../../api';
import { hours } from './hours';
import { AppointmentBlock } from '../../api/calendar/entities/appointment-block';
import CalendarBlock from '@/modules/app/components/calendar/calendar-block.vue';
import { duration } from 'moment';

@Component({
    name: 'calendar-day-view',
    components: {
        CalendarBlock
    }
})
export default class CalendarDayView extends Vue {
    get hours() {
        return hours;
    }

    get date() {
        const calendarStore = getModule(CalendarStore, this.$store);
        return calendarStore.date;
    }

    get pendingBlocks() {
        const calendarStore = getModule(CalendarStore, this.$store);
        return calendarStore.pendingBlocks;
    }

    get resizingBlock() {
        return this.pendingBlocks.find(b => b.meta.resizing)!;
    }

    unsub: (() => void) | null = null;
    hoursOfOp: HoursOfOperationDay | null = null;

    isMouseDown: boolean = false;

    async mounted() {
        const calendarStore = getModule(CalendarStore, this.$store);
        const settingsStore = getModule(SettingsStore, this.$store);

        await settingsStore.init();

        this.unsub = store.subscribe((mut, s) => {
            if (mut.type == 'calendar/SET_DATE') {
                this.scrollToOpenHour(mut.payload);
            }
        });

        this.scrollToOpenHour(calendarStore.date);

        window.addEventListener('mouseup', this.onDragEnd);
        window.addEventListener('click', this.onDragClick);
    }

    beforeDestroy() {
        if (this.unsub != null) {
            this.unsub();
        }

        window.removeEventListener('mouseup', this.onDragEnd);
        window.removeEventListener('click', this.onDragClick);
    }

    getBlock(time: number) {
        const calendarStore = getModule(CalendarStore, this.$store);
        return this.pendingBlocks.find(b => b.time == time);
    }

    onBlockDragStart(el: Event) {
        (el.target as HTMLDivElement).classList.add('is-dragging');
        console.log(el.target);
    }

    onBlockDragEnd(el: Event) {
        (el.target as HTMLDivElement).classList.remove('is-dragging');
    }

    onDragStart(time: number) {
        this.isMouseDown = true;

        // assume user wants to do X:00 or X:30
        time -= time % 30;

        const block = new AppointmentBlock(this.date, time, 30, { pending: true, resizing: true });
        block.meta.initialTime = time;

        const calendarStore = getModule(CalendarStore, this.$store);
        calendarStore.ADD_BLOCK(block);
    }

    onDragUpdate(time: number) {
        if (!this.isMouseDown) {
            return;
        }

        const calendarStore = getModule(CalendarStore, this.$store);

        // Down
        if (this.resizingBlock.time < time) {
            // Going down, but we went up first
            if (this.resizingBlock.meta.initialTime > this.resizingBlock.time) {
                calendarStore.RESIZE_BLOCK({
                    block: this.resizingBlock,
                    time: time,
                    duration: this.resizingBlock.meta.initialTime - time
                });
            } else {
                calendarStore.RESIZE_BLOCK({
                    block: this.resizingBlock,
                    time: this.resizingBlock.meta.initialTime,
                    duration: time - this.resizingBlock.meta.initialTime
                });
            }
        }
        // Up
        else {
            calendarStore.RESIZE_BLOCK({
                block: this.resizingBlock,
                time: time,
                duration: this.resizingBlock.meta.initialTime - time
            });
        }
    }

    onDragEnd() {
        console.log('mouseUp');
        if (!this.isMouseDown) {
            return;
        }

        this.isMouseDown = false;

        const calendarStore = getModule(CalendarStore, this.$store);
        calendarStore.REMOVE_RESIZING_FLAG(this.resizingBlock);
    }

    onDragClick() {
        // if no mouse up event occured, force stop the dragging
        if (this.isMouseDown) {
            this.isMouseDown = false;
        }
    }

    scrollToOpenHour(date: Date) {
        const settingsStore = getModule(SettingsStore, this.$store);
        this.hoursOfOp = settingsStore.hoursOfOperation.getHoursForDay(date.getDay())!;

        // If hours of operation are set for the day, auto scroll to the first hour.
        if (this.hoursOfOp != null) {
            let openHour = Math.floor(this.hoursOfOp.open / 60) - 1;
            const openPeriod = this.hoursOfOp.open >= 720 ? 'pm' : 'am';

            const ref = this.$refs.dayView as HTMLDivElement;

            if (ref == null) {
                return;
            }

            const hourElement = ref.querySelector(`#hour-${openHour}-${openPeriod}`);

            hourElement!.scrollIntoView();
        }
    }

    onPreviousSwipe() {
        const calendarStore = getModule(CalendarStore, this.$store);
        calendarStore.adjustDate({ direction: 'previous', step: 'day' });
    }

    onNextSwipe() {
        const calendarStore = getModule(CalendarStore, this.$store);
        calendarStore.adjustDate({ direction: 'next', step: 'day' });
    }

    determineHourBackground(raw: number) {
        if (this.hoursOfOp != null && this.hoursOfOp.containsTime(raw)) {
            return 'hour is-flex-grow-1 has-background-white';
        }

        return 'hour is-flex-grow-1 has-background-white-bis';
    }
}
</script>
