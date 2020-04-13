<template>
    <div>
        <!-- Header -->
        <div>
            <div class="is-flex is-flex-row is-justify-content-space-between is-align-items-center">
                <div class="has-text-weight-bold is-size-5">{{ title }}</div>

                <div class="is-flex is-flex-row is-align-items-center">
                    <b-button
                        type="is-text"
                        icon-left="chevron-left"
                        @click="onPrevious"
                        title="Previous month"
                    />
                    <b-button
                        type="is-text"
                        icon-left="calendar-today"
                        @click="onToday"
                        title="Go to today"
                    />
                    <b-button
                        type="is-text"
                        icon-left="chevron-right"
                        @click="onNext"
                        title="Next month"
                    />
                </div>
            </div>

            <div class="days">
                <div v-for="d in [0, 1, 2, 3, 4, 5, 6]" :key="d">
                    <div
                        class="has-text-weight-bold has-text-right"
                        style="padding: 4px;"
                    >{{ dayNames[d] }}</div>

                    <div v-for="w in [0, 1, 2, 3, 4, 5, 6]" :key="w">
                        <div :class="getDayStyles(w,d)" style="height: 24px; text-align: end;">
                            <a @click="onDay(w, d)">{{ getDayNumber(w, d) }}</a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<style lang="sass" scoped>
.day

    a
        padding: 4px
        color: $dark

    &.is-selected
        background-color: lighten($primary, 10%)!important
        border-radius: 4px

        a
            color: $white!important

    &.is-today
        border-radius: 4px
        background-color: $grey-lighter

.days
    display: flex
    flex-direction: row
    justify-content: space-between

    > div
        width: calc(100% / 7) // fuck it

</style>

<script lang="ts">
import { Component, Vue, Prop, Watch } from 'vue-property-decorator';
import moment, { Moment } from 'moment';
import { getModule } from 'vuex-module-decorators';
import CalendarStore from '../../store/calendar/calendar-store';

@Component({
    name: 'calendar-datepicker'
})
export default class CalendarDatepicker extends Vue {
    get title() {
        return this.viewing.format('MMMM YYYY');
    }

    get value() {
        const calendarStore = getModule(CalendarStore, this.$store);
        return calendarStore.date;
    }

    set value(v: Date) {
        const calendarStore = getModule(CalendarStore, this.$store);
        calendarStore.jumpDate(v);
    }

    viewing: Moment = moment();

    dayNames = ['Su', 'Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa'];
    days: number[] = [];
    offset: number = 0;

    mounted() {
        this.generateDays();
    }

    getDayStyles(week: number, day: number) {
        const today = moment();
        const rawOffset = week * 7 + day;
        const number = this.days[rawOffset - this.offset];

        let classes = 'day';

        const calendarStore = getModule(CalendarStore, this.$store);
        const period = calendarStore.view == 'day' ? 1 : 7;
        const offset = number - this.value.getDate();

        // Is the date actively being viewed?
        if (
            offset < period &&
            offset >= 0 &&
            this.value.getMonth() == this.viewing.month() &&
            this.value.getFullYear() == this.viewing.year()
        ) {
            classes += ' is-selected';
        }

        //Is the date today?
        if (number == today.date() && this.viewing.month() == today.month() && this.viewing.year() == today.year()) {
            classes += ' is-today';
        }

        return classes;
    }

    getDayNumber(week: number, day: number) {
        const rawOffset = week * 7 + day;
        const number = this.days[rawOffset - this.offset];

        if (number == null) {
            return '';
        } else {
            return number.toString();
        }
    }

    onDay(week: number, day: number) {
        const rawOffset = week * 7 + day;
        const number = this.days[rawOffset - this.offset];
        this.value = this.viewing
            .startOf('month')
            .add(number - 1, 'days')
            .toDate();

        this.generateDays();
    }

    onToday() {
        this.viewing = moment();
        this.generateDays();
    }

    onNext() {
        this.viewing = moment(this.viewing).add(1, 'month');
        this.generateDays();
    }

    onPrevious() {
        this.viewing = moment(this.viewing).subtract(1, 'month');
        this.generateDays();
    }

    generateDays() {
        this.days.length = 0;
        const dayCount = this.viewing.daysInMonth();

        for (let i = 1; i <= dayCount; i++) {
            this.days.push(i);
        }

        this.offset = moment(this.viewing)
            .startOf('month')
            .day();
    }
}
</script>