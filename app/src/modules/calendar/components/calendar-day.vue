<template>
    <div
        class="calendar has-border-right has-border-top has-border-left is-flex is-flex-row is-flex-grow-1 is-relative is-unselectable"
        ref
        v-touch:swipe.right="onPreviousSwipe"
        v-touch:swipe.left="onNextSwipe"
        style="margin-bottom: 128px!important"
    >
        <!-- Axis -->
        <div class="is-flex is-flex-column has-w-40px has-border-right">
            <div
                class="has-h-80px has-border-bottom"
                v-for="hour in hours"
                :key="`${hour.hour}-${hour.period}-key`"
            >
                <div class="is-flex is-flex-row is-justify-content-end has-padding-right-1">
                    <span class="is-size-6">{{ hour.hour }}</span>
                    <span class="is-size-7 has-text-grey">{{ hour.period }}</span>
                </div>
            </div>
        </div>

        <!-- Hours -->
        <div class="is-flex is-flex-column is-flex-grow-1 is-relative" ref="hours">
            <div
                class="has-h-80px has-border-bottom"
                v-for="hour in hours"
                :key="`${hour.hour}-${hour.period}`"
                :id="`hour-${hour.hour}-${hour.period}`"
            >
                <div :class="determineHourBackground(hour.raw)">
                    <div
                        v-for="i in [0,1,2,3]"
                        :key="i"
                        class="interval has-h-20px"
                        @mousedown.left.self="onCreateDragStart(hour.raw + i * 15)"
                    >&nbsp;</div>
                </div>
            </div>

            <!-- Appointment blocks -->
            <div class="blocks">
                <calendar-block
                    v-for="block in blocks"
                    :key="block.id"
                    :ref="`block-${block.time}`"
                    :value="block"
                />
            </div>
        </div>
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { hours } from './hours';
import CalendarBlock from '@/modules/calendar/components/calendar-block.vue';
import moment, { duration } from 'moment';
import Calendar from '../mixins/calendar';
import calendarStore from '../store/calendar-store';
import { HoursOfOperationDay } from '@/modules/settings/api/entities/hours-of-operation-day';
import settingsStore from '@/modules/settings/store/settings-store';
import store from '@/core/store';

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

    get blocks() {
        return calendarStore.blocksForDay(this.date);
    }

    unsub: (() => void) | null = null;
    hoursOfOp: HoursOfOperationDay | null = null;
    currentAction: 'creating-block' | 'moving-block' | 'resizing-block' | null = null;

    async mounted() {
        await settingsStore.init();

        this.loadAppointments(this.date, 'day');

        this.unsub = store.subscribe((mut, s) => {
            if (mut.type == 'calendar/SET_DATE') {
                this.scrollToOpenHour(mut.payload);
                this.loadAppointments(this.date, 'day');
            }
        });

        this.scrollToOpenHour(calendarStore.date);
    }

    beforeDestroy() {
        if (this.unsub != null) {
            this.unsub();
        }
    }

    onCreateDragStart(time: number) {
        this.currentAction = 'creating-block';

        // assume user wants to do X:00 or X:30
        time -= time % 30;
        const b = this.createBlock(this.date, time, 30);

        // Trigger the "hold" event on the component.
        this.$nextTick(() => {
            const comp = this.$refs[`block-${b.time}`];
            (comp as any[])[0].focus();
        });
    }

    scrollToOpenHour(date: Date) {
        this.hoursOfOp = settingsStore.hoursOfOperation.getHoursForDay(date.getDay())!;

        // If hours of operation are set for the day, auto scroll to the first hour.
        if (this.hoursOfOp != null) {
            let openHour = Math.floor(this.hoursOfOp.open / 60) - 1;
            const openPeriod = this.hoursOfOp.open >= 720 ? 'pm' : 'am';

            const ref = this.$refs.hours as HTMLDivElement;

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
