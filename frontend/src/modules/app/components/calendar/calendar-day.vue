<template>
    <div
        class="calendar has-border-right has-border-top has-border-left is-flex is-flex-column is-flex-grow-1 is-relative is-unselectable"
        ref="dayView"
        v-touch:swipe.right="onPreviousSwipe"
        v-touch:swipe.left="onNextSwipe"
        style="margin-bottom: 128px!important"
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
                    @mousedown.left.self.stop="onCreateDragStart(hour.raw + i * 15)"
                    @mouseover.left.self.stop="onMouseOverInterval(hour.raw + i * 15)"
                >
                    <calendar-block
                        @moveStart="onBlockMoveStart(hour.raw + i * 15)"
                        @resizeStart="onBlockResizeStart(hour.raw + i * 15)"
                        @delete="onBlockDelete(hour.raw + i * 15)"
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
import moment, { duration } from 'moment';
import calendarStore from '../../store/calendar/calendar-store';
import settingsStore from '../../store/settings/settings-store';
import Calendar from '@/modules/app/mixins/calendar/calendar';

@Component({
    name: 'calendar-day',
    components: {
        CalendarBlock
    }
})
export default class CalendarDay extends Calendar {
    get hours() {
        return hours;
    }

    get pendingBlocks() {
        return calendarStore.pendingBlocks;
    }

    get modifyingBlock() {
        return this.pendingBlocks.find(b => b.meta.modifying)!;
    }

    unsub: (() => void) | null = null;
    hoursOfOp: HoursOfOperationDay | null = null;
    currentAction: 'creating-block' | 'moving-block' | 'resizing-block' | null = null;

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

    onBlockMoveStart(time: number) {
        if (this.currentAction != null) {
            return;
        }

        const block = this.getBlock(time)!;
        this.addModifyingFlag(block);
        this.currentAction = 'moving-block';
    }

    onBlockDragEnd() {
        if (this.currentAction != 'moving-block') {
            return;
        }

        this.removeModifyingFlag(this.modifyingBlock);
        this.currentAction = 'moving-block';
    }

    onBlockResizeStart(time: number) {
        if (this.currentAction != null) {
            return;
        }

        const block = this.getBlock(time)!;
        this.addModifyingFlag(block);
        this.currentAction = 'resizing-block';
    }

    onBlockDelete(time: number) {
        const block = this.getBlock(time)!;
        this.deleteBlock(block);
    }

    onCreateDragStart(time: number) {
        this.currentAction = 'creating-block';

        // assume user wants to do X:00 or X:30
        time -= time % 30;
        this.createBlock(this.date, time, 30);
    }

    onMouseOverInterval(time: number) {
        if (this.currentAction == 'creating-block' || this.currentAction == 'resizing-block') {
            this.resizeBlock(this.modifyingBlock, time);
        } else if (this.currentAction == 'moving-block') {
            this.moveBlock(this.modifyingBlock, time);
        }
    }

    onMouseUp() {
        if (this.currentAction != null) {
            this.currentAction = null;
            this.saveBlockChanges(this.modifyingBlock);
        }
    }

    onCreateDragClick() {
        // if no mouse up event occured, force stop the dragging
        if (this.currentAction == 'creating-block') {
            this.currentAction = null;

            this.removeModifyingFlag(this.modifyingBlock);
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
