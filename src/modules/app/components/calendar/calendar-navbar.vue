<template>
    <div>
        <div class="has-padding-y-2 has-background-white has-w-100 is-fixed has-background-light">
            <div
                class="is-flex is-flex-row is-justify-content-space-between is-align-items-center"
                style="height: 41.5px;"
            >
                <a
                    class="has-text-decoration-none has-text-left has-text-dark has-margin-left-3 is-flex is-flex-row is-align-items-center"
                    type="is-text"
                    @click="$refs.datepickerModal.show()"
                    title="Select date"
                >
                    <b-button
                        icon-left="calendar"
                        type="is-text"
                        size="is-medium"
                        class="is-hidden-mobile has-padding-right-2"
                        title="Select date"
                    />

                    <div class="is-flex is-flex-row-tablet is-flex-column-mobile">
                        <p
                            class="is-size-7 is-size-6-tablet has-margin-right-1-tablet"
                        >{{ dateTitle }}</p>
                        <span class="is-size-6-mobile">{{ dateDescription }}</span>
                    </div>
                </a>

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

                    <b-field class="has-margin-bottom-0">
                        <b-radio-button
                            v-model="calendarView"
                            size="is-small"
                            native-value="day"
                            title="Switch to day view"
                        >Day</b-radio-button>
                        <b-radio-button
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
                    </b-field>

                    <b-button
                        icon-left="calendar-plus"
                        type="is-text"
                        size="is-medium"
                        title="Create new appointment"
                    />
                </div>
            </div>

            <calendar-datepicker-modal ref="datepickerModal" />
        </div>

        <div style="height:57.5px!important">&nbsp;</div>
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { getModule } from 'vuex-module-decorators';
import CalendarStore from '../../store/calendar/calendar-store';
import { CalendarView } from '../../store/calendar/calendar-view';
import CalendarDatepickerModal from './calendar-datepicker-modal.vue';
import moment from 'moment';

@Component({
    name: 'calendar-navbar',
    components: {
        CalendarDatepickerModal
    }
})
export default class CalendarNavbar extends Vue {
    get calendarView() {
        const calendarStore = getModule(CalendarStore, this.$store);
        return calendarStore.view;
    }

    set calendarView(val: CalendarView) {
        const calendarStore = getModule(CalendarStore, this.$store);
        calendarStore.changeView(val);
    }

    get dateTitle() {
        const calendarStore = getModule(CalendarStore, this.$store);
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
        const calendarStore = getModule(CalendarStore, this.$store);
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
        const calendarStore = getModule(CalendarStore, this.$store);
        calendarStore.adjustDate({ direction: 'previous', step: calendarStore.view });
    }

    onTodayClick() {
        const calendarStore = getModule(CalendarStore, this.$store);
        calendarStore.jumpDate(new Date());
    }

    onNextClick() {
        const calendarStore = getModule(CalendarStore, this.$store);
        calendarStore.adjustDate({ direction: 'next', step: calendarStore.view });
    }
}
</script>