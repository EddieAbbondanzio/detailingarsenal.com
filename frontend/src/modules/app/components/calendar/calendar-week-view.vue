<template>
    <div class="week is-flex is-flex-column is-flex-grow-1" ref="dayView">
        <div
            class="hour-row is-flex is-flex-row has-background-white"
            style="position: sticky; top: 0px;"
        >
            <div class="hour-key has-w-40px">&nbsp;</div>

            <div
                style="flex: 1 1 0"
                class="day is-flex is-flex-row is-justify-content-center"
                v-for="day in days"
                :key="day.day"
            >
                <span
                    class="is-hidden-widescreen"
                >{{ day.name.substring(0,2) }}&nbsp;{{ day.date.getDate() }}</span>
                <span
                    class="is-hidden-touch is-hidden-desktop-only"
                >{{ day.name }}&nbsp;{{ day.date.getDate() }}</span>
            </div>
        </div>

        <div
            class="hour-row has-h-80px is-flex is-flex-row"
            v-for="hour in hours"
            :key="`${hour.hour}-${hour.period}`"
            :id="`block-${hour.hour}-${hour.period}`"
        >
            <!-- Axis -->
            <div
                class="hour-key is-flex is-flex-row has-w-40px is-justify-content-end has-padding-right-1"
            >
                <span class="is-size-6">{{ hour.hour }}</span>
                <span class="is-size-7 has-text-grey">{{ hour.period }}</span>
            </div>

            <div class="day is-flex is-flex-row is-flex-grow-1" v-for="day in days" :key="day.day">
                <div :class="determineBlockBackground(day.day, hour.raw)">&nbsp;</div>
            </div>
        </div>
    </div>
</template>

<style lang="sass" scoped>
.week
    border-left: 1px solid $grey-lighter
    border-top: 1px solid $grey-lighter
    border-bottom: 1px solid $grey-lighter

    .hour-row
        border-bottom: 1px solid $grey-lighter

    .day
        border-right: 1px solid $grey-lighter


.hour-key
    border-right: 1px solid $grey-lighter

.hour-block
    border-bottom: 1px solid $grey-lighter
    border-left: 1px solid $grey-lighter
    border-right: 1px solid $grey-lighter

.has-h-40px
    height: 40px

.has-w-40px
    width: 40px

.has-h-80px
    height: 80px
</style>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { hours } from './hours';
import { days } from './days';
import { getModule } from 'vuex-module-decorators';
import CalendarStore from '../../store/calendar/calendar-store';
import SettingsStore from '../../store/settings/settings-store';
import store from '../../../../core/store';
import moment from 'moment';

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
        const calendarStore = getModule(CalendarStore, this.$store);
        const settingsStore = getModule(SettingsStore, this.$store);

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

    scrollToOpenHour() {
        const settingsStore = getModule(SettingsStore, this.$store);
        const earliestHour = settingsStore.hoursOfOperation.getEarliestOpening();

        // If hours of operation are set for the day, auto scroll to the first hour.
        let openHour = Math.floor(earliestHour / 60);
        const openPeriod = earliestHour >= 720 ? 'pm' : 'am';

        const hourElement = (this.$refs.dayView as HTMLDivElement).querySelector(`#block-${openHour}-${openPeriod}`);

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
        const settingsStore = getModule(SettingsStore, this.$store);
        const hoursOfOp = settingsStore.hoursOfOperation.getHoursForDay(day);

        if (hoursOfOp != null && hoursOfOp.containsTime(hour)) {
            return 'is-flex-grow-1 has-background-white';
        }

        return 'is-flex-grow-1 has-background-white-bis';
    }
}
</script>