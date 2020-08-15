<template>
    <div>
        <div class="has-padding-y-2 has-background-white" style="z-index: 20;">
            <div
                class="is-flex is-flex-row is-justify-content-space-between is-align-items-center"
                style="height: 41.5px;"
            >
                <!-- Touch date header -->
                <a
                    class="has-text-decoration-none has-text-left has-text-dark is-flex is-flex-row is-align-items-center has-margin-left-3 is-hidden-desktop"
                    type="is-text"
                    @click="$refs.datepickerModal.show()"
                    title="Select date"
                >
                    <div class="is-flex is-flex-column">
                        <p class="is-size-7 is-size-6-tablet">{{ dateTitle }}</p>
                        <span class="is-size-6-mobile">{{ dateDescription }}</span>
                    </div>
                </a>

                <!-- Desktop date header -->
                <div class="is-flex is-flex-row is-hidden-touch has-margin-x-3">
                    <p
                        class="is-size-4 is-size-5-tablet has-text-weight-bold"
                    >{{ dateTitle }}&nbsp;{{ dateDescription }}</p>
                </div>

                <div class="is-flex is-flex-row is-align-items-center is-justify-content-end">
                    <div
                        class="buttons has-addons has-padding-right-3 is-flex is-align-items-center has-margin-bottom-0 is-hidden-mobile"
                    >
                        <b-button
                            class="has-margin-bottom-0"
                            outlined
                            size="is-small"
                            icon-left="chevron-left"
                            @click="onPreviousClick"
                            :title="`Previous ${calendarView}`"
                        />
                        <b-button
                            class="has-margin-bottom-0"
                            outlined
                            size="is-small"
                            title="Go to today"
                            @click="onTodayClick"
                        >Today</b-button>
                        <b-button
                            class="has-margin-bottom-0"
                            outlined
                            size="is-small"
                            icon-left="chevron-right"
                            @click="onNextClick"
                            :title="`Next ${calendarView}`"
                        />
                    </div>

                    <!-- <b-field class="has-margin-bottom-0 has-margin-right-3-desktop">
                        <b-radio-button
                            v-model="calendarView"
                            size="is-small"
                            native-value="day"
                            title="Switch to day view"
                        >Day</b-radio-button>
                        <b-radio-button12
                            v-model="calendarView"
                            size="is-small"
                            native-value="week"
                            title="Switch to week view"
                        >Week</b-radio-button>
                        <b-radio-button
                            v-model="calendarView"
                            size="is-small"
                            native-value="month"
                            title="Switch to month view"
                        >Month</b-radio-button>
                    </b-field>-->

                    <b-button
                        class="is-hidden-desktop"
                        icon-left="calendar-plus"
                        type="is-text"
                        size="is-medium"
                        title="Create new appointment"
                        @click="onCreateClick"
                    />
                </div>
            </div>

            <calendar-datepicker-modal ref="datepickerModal" />
        </div>
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import CalendarDatepickerModal from './calendar-datepicker-modal.vue';
import moment from 'moment';
import { CalendarView } from '../store/calendar-view';
import calendarStore from '../store/calendar-store';

@Component({
    name: 'calendar-navbar',
    components: {
        CalendarDatepickerModal
    }
})
export default class CalendarNavbar extends Vue {
    get calendarView() {
        return calendarStore.view;
    }

    set calendarView(val: CalendarView) {
        calendarStore.changeView(val);
    }

    get dateTitle() {
        switch (calendarStore.view) {
            case 'day':
                return 'Day of ';
            case 'week':
                return 'Week of';
            case 'month':
                return 'Month of';
            default:
                throw new Error();
        }
    }

    get dateDescription() {
        switch (calendarStore.view) {
            case 'day':
                return moment(calendarStore.date).format('MMMM Do YYYY');
            case 'week':
                return moment(calendarStore.date).format('MMMM Do YYYY');
            case 'month':
                return moment(calendarStore.date).format('MMMM YYYY');
            default:
                throw new Error();
        }
    }

    onPreviousClick() {
        calendarStore.adjustDate({ direction: 'previous', step: calendarStore.view });
    }

    onTodayClick() {
        calendarStore.jumpDate(new Date());
    }

    onNextClick() {
        calendarStore.adjustDate({ direction: 'next', step: calendarStore.view });
    }

    onCreateClick() {
        calendarStore.SET_CREATE_STEP('details');
    }
}
</script>
