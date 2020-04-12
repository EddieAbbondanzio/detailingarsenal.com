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
                    <div class="has-text-weight-bold" style="padding: 4px;">{{ dayNames[d] }}</div>

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
        background-color: lighten($primary, 10%)
        border-radius: 4px

        a
            color: $white!important

.days
    display: flex
    flex-direction: row
    justify-content: space-between
</style>

<script lang="ts">
import { Component, Vue, Prop, Watch } from 'vue-property-decorator';
import moment, { Moment } from 'moment';

@Component({
    name: 'calendar-datpicker'
})
export default class Calendarvaluepicker extends Vue {
    get title() {
        return this.value.format('MMMM YYYY');
    }

    value: Moment = moment();

    dayNames = ['Su', 'Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa'];
    days: number[] = [];
    offset: number = 0;

    mounted() {
        this.generateDays();
    }

    getDayStyles(week: number, day: number) {
        const rawOffset = week * 7 + day;
        const number = this.days[rawOffset - this.offset];

        if (this.value.date() == number) {
            return 'day is-selected';
        } else {
            return 'day';
        }
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
        this.value = this.value.startOf('month').add(number - 1, 'days');
        this.generateDays();
    }

    onToday() {
        this.value = moment();
        this.generateDays();
    }

    onNext() {
        this.value = moment(this.value).add(1, 'month');
        this.generateDays();
    }

    onPrevious() {
        this.value = moment(this.value).subtract(1, 'month');
        this.generateDays();
    }

    generateDays() {
        this.days.length = 0;
        const dayCount = this.value.daysInMonth();

        for (let i = 1; i <= dayCount; i++) {
            this.days.push(i);
        }

        this.offset = moment(this.value)
            .startOf('month')
            .day();
    }
}
</script>