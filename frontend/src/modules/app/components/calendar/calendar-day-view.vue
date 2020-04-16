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
                    @mousedown.self.stop="onCreateDragStart(hour.raw + i * 15)"
                    @mouseover.self.stop="onMouseOverInterval(hour.raw + i * 15)"
                >
                    <calendar-block
                        @mousedown.self.stop="onBlockDragStart(hour.raw + i * 15)"
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
import SettingsStore from '../../store/settings/settings-store';
import CalendarStore from '../../store/calendar/calendar-store';
import store from '../../../../core/store';
import { HoursOfOperationDay } from '../../api';
import { hours } from './hours';
import { AppointmentBlock } from '../../api/calendar/entities/appointment-block';
import CalendarBlock from '@/modules/app/components/calendar/calendar-block.vue';
import { duration } from 'moment';
import calendarStore from '../../store/calendar/calendar-store';
import settingsStore from '../../store/settings/settings-store';

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
        return calendarStore.date;
    }

    get pendingBlocks() {
        return calendarStore.pendingBlocks;
    }

    get modifyingBlock() {
        return this.pendingBlocks.find(b => b.meta.modifying)!;
    }

    unsub: (() => void) | null = null;
    hoursOfOp: HoursOfOperationDay | null = null;
    currentAction: 'creating-block' | 'moving-block' | null = null;

    async mounted() {
        await settingsStore.init();

        this.unsub = store.subscribe((mut, s) => {
            if (mut.type == 'calendar/SET_DATE') {
                this.scrollToOpenHour(mut.payload);
            }
        });

        this.scrollToOpenHour(calendarStore.date);

        window.addEventListener('mouseup', this.onMouseUp);
        window.addEventListener('click', this.onCreateDragClick);
    }

    beforeDestroy() {
        if (this.unsub != null) {
            this.unsub();
        }

        window.removeEventListener('mouseup', this.onMouseUp);
        window.removeEventListener('click', this.onCreateDragClick);
    }

    getBlock(time: number) {
        return this.pendingBlocks.find(b => b.time == time);
    }

    onBlockDragStart(time: number) {
        if (this.currentAction != null) {
            return;
        }

        const block = this.getBlock(time);
        calendarStore.ADD_BLOCK_META({ block: this.modifyingBlock, meta: { name: 'modifying', value: true } });

        this.currentAction = 'moving-block';
    }

    onBlockDragEnd() {
        if (this.currentAction != 'moving-block') {
            return;
        }

        calendarStore.REMOVE_BLOCK_META({ block: this.modifyingBlock, name: 'modifying' });

        this.currentAction = 'moving-block';
    }

    onCreateDragStart(time: number) {
        this.currentAction = 'creating-block';

        // assume user wants to do X:00 or X:30
        time -= time % 30;

        const block = new AppointmentBlock(this.date, time, 30, { pending: true, modifying: true });
        block.meta.initialTime = time;

        calendarStore.ADD_BLOCK(block);
    }

    onMouseOverInterval(time: number) {
        if (this.currentAction == 'creating-block') {
            // Down
            if (this.modifyingBlock.time < time) {
                // Going down, but we went up first
                if (this.modifyingBlock.meta.initialTime > this.modifyingBlock.time) {
                    calendarStore.RESIZE_BLOCK({
                        block: this.modifyingBlock,
                        time: time,
                        duration: this.modifyingBlock.meta.initialTime - time
                    });
                } else {
                    calendarStore.RESIZE_BLOCK({
                        block: this.modifyingBlock,
                        time: this.modifyingBlock.meta.initialTime,
                        duration: time - this.modifyingBlock.meta.initialTime
                    });
                }
            }
            // Up
            else {
                calendarStore.RESIZE_BLOCK({
                    block: this.modifyingBlock,
                    time: time,
                    duration: this.modifyingBlock.meta.initialTime - time
                });
            }
        } else if (this.currentAction == 'moving-block') {
            calendarStore.MOVE_BLOCK({
                block: this.modifyingBlock,
                time: time
            });
        }
    }

    onMouseUp() {
        if (this.currentAction == 'creating-block' || this.currentAction == 'moving-block') {
            this.currentAction = null;

            calendarStore.REMOVE_BLOCK_META({ block: this.modifyingBlock, name: 'modifying' });
        }
    }

    onCreateDragClick() {
        // if no mouse up event occured, force stop the dragging
        if (this.currentAction == 'creating-block') {
            this.currentAction = null;

            calendarStore.REMOVE_BLOCK_META({ block: this.modifyingBlock, name: 'modifying' });
        }
    }

    scrollToOpenHour(date: Date) {
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
        calendarStore.adjustDate({ direction: 'previous', step: 'day' });
    }

    onNextSwipe() {
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
