<template>
    <div
        class="calendar is-flex is-flex-column is-flex-grow-1 has-border-right has-border-top has-border-left is-unselectable"
        ref="weekView"
    >
        <div
            class="has-border-bottom is-flex is-flex-row has-background-white"
            style="position: sticky; top: 0px;"
        >
            <div class="hour-key has-w-40px">&nbsp;</div>

            <div
                style="flex: 1 1 0"
                class="has-border-left day is-flex is-flex-row is-justify-content-center"
                v-for="day in days"
                :key="day.day"
            >
                <span
                    class="is-hidden-widescreen is-size-7-mobile"
                >{{ day.name.substring(0,2) }}&nbsp;{{ day.date.getDate() }}</span>
                <span
                    class="is-hidden-touch is-hidden-desktop-only"
                >{{ day.name }}&nbsp;{{ day.date.getDate() }}</span>
            </div>
        </div>

        <div
            class="has-border-bottom has-h-80px is-flex is-flex-row"
            v-for="hour in hours"
            :key="`${hour.hour}-${hour.period}`"
            :id="`block-${hour.hour}-${hour.period}`"
        >
            <!-- Axis -->
            <div class="is-flex is-flex-row has-w-40px is-justify-content-end has-padding-right-1">
                <span class="is-size-6">{{ hour.hour }}</span>
                <span class="is-size-7 has-text-grey">{{ hour.period }}</span>
            </div>

            <div
                class="has-border-left is-flex is-flex-row is-flex-grow-1"
                v-for="day in days"
                :key="day.day"
            >
                <div :class="determineBlockBackground(day.day, hour.raw)">&nbsp;</div>
            </div>
        </div>
    </div>
</template>


<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { hours } from './hours';
import { days } from './days';
import moment from 'moment';
import settingsStore from '../../store/settings/settings-store';
import calendarStore from '../../store/calendar/calendar-store';
import store from '../../../../core/store';

@Component({
    name: 'calendar-week-view'
})
export default class CalendarWeekView extends Vue {
    get hours() {
        return hours;
    }

    days: { day: number; name: string; date: Date }[] = [];
    unsub: (() => void) | null = null;

    async mounted() {
        await settingsStore.init();

        this.unsub = store.subscribe((mut, s) => {
            if (mut.type == 'calendar/SET_DATE') {
                this.generateDates(mut.payload);
                this.scrollToOpenHour();
            }
        });

        this.generateDates(calendarStore.date);
        this.scrollToOpenHour();
    }

    beforeDestroy() {
        if (this.unsub != null) {
            this.unsub();
        }
    }

    scrollToOpenHour() {
        const earliestHour = settingsStore.hoursOfOperation.getEarliestOpening();

        // If hours of operation are set for the day, auto scroll to the first hour.
        let openHour = Math.floor(earliestHour / 60) - 1;
        const openPeriod = earliestHour >= 720 ? 'pm' : 'am';

        const ref = this.$refs.weekView as HTMLDivElement;

        if (ref == null) {
            return;
        }

        const hourElement = ref.querySelector(`#block-${openHour}-${openPeriod}`);

        hourElement!.scrollIntoView();
    }

    generateDates(date: Date) {
        this.days.length = 0;
        const m = moment(date);

        for (let i = 0; i < days.length; i++) {
            const date = m.add(1, 'day').toDate();
            const d = days[i];

            this.days.push({ day: d.day, date, name: d.name });
        }
    }

    determineBlockBackground(day: number, hour: number) {
        const hoursOfOp = settingsStore.hoursOfOperation.getHoursForDay(day);

        if (hoursOfOp != null && hoursOfOp.containsTime(hour)) {
            return 'is-flex-grow-1 has-background-white';
        }

        return 'is-flex-grow-1 has-background-white-bis';
    }
}
</script>