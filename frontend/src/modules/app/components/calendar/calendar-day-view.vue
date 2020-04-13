<template>
    <div
        class="day-view is-flex is-flex-column is-flex-grow-1"
        ref="dayView"
        v-touch:swipe.right="onPreviousSwipe"
        v-touch:swipe.left="onNextSwipe"
    >
        <div
            class="hour-block has-h-80px is-flex is-flex-row"
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

            <!-- Block -->
            <div :class="determineBlockBackground(hour.raw)">&nbsp;</div>
        </div>
    </div>
</template>

<style lang="sass" scoped>
.day-view
    border-top: 1px solid $grey-lighter

.hour-key
    border-right: 1px solid $grey-lighter

.hour-block
    border-bottom: 1px solid $grey-lighter
    border-left: 1px solid $grey-lighter
    border-right: 1px solid $grey-lighter

.has-w-40px
    width: 40px

.has-h-80px
    height: 80px
</style>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { getModule } from 'vuex-module-decorators';
import SettingsStore from '../../store/settings/settings-store';
import CalendarStore from '../../store/calendar/calendar-store';
import store from '../../../../core/store';
import { HoursOfOperationDay } from '../../api';
import { hours } from './hours';

@Component({
    name: 'calendar-day-view'
})
export default class CalendarDayView extends Vue {
    get hours() {
        return hours;
    }

    unsub: (() => void) | null = null;
    hoursOfOp: HoursOfOperationDay | null = null;

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
    }

    beforeDestroy() {
        if (this.unsub != null) {
            this.unsub();
        }
    }

    scrollToOpenHour(date: Date) {
        const settingsStore = getModule(SettingsStore, this.$store);
        this.hoursOfOp = settingsStore.hoursOfOperation.getHoursForDay(date.getDay())!;

        // If hours of operation are set for the day, auto scroll to the first hour.
        if (this.hoursOfOp != null) {
            let openHour = Math.floor(this.hoursOfOp.open / 60);
            const openPeriod = this.hoursOfOp.open >= 720 ? 'pm' : 'am';

            const hourElement = (this.$refs.dayView as HTMLDivElement).querySelector(
                `#block-${openHour}-${openPeriod}`
            );

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

    determineBlockBackground(raw: number) {
        if (this.hoursOfOp != null && this.hoursOfOp.containsTime(raw)) {
            return 'is-flex-grow-1 has-background-white';
        }

        return 'is-flex-grow-1 has-background-white-bis';
    }
}
</script>
