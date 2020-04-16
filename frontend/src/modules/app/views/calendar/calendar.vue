<template>
    <page background="is-white">
        <template v-slot:header>
            <page-header>
                <calendar-navbar />
            </page-header>
        </template>

        <template v-slot:sidebar>
            <calendar-sidebar />
        </template>

        <calendar-day-view v-if="view == 'day'" />
        <calendar-week-view v-if="view == 'week'" />
    </page>
</template>

<style lang="sass" scoped>

</style>

<script lang="ts">
import { Component, Vue, Prop, Ref } from 'vue-property-decorator';
import CalendarNavbar from '@/modules/app/components/calendar/calendar-navbar.vue';
import CalendarDayView from '@/modules/app/components/calendar/calendar-day-view.vue';
import CalendarWeekView from '@/modules/app/components/calendar/calendar-week-view.vue';
import CalendarSidebar from '@/modules/app/components/calendar/calendar-sidebar.vue';
import calendarStore from '../../store/calendar/calendar-store';

@Component({
    name: 'calendar',
    components: {
        CalendarNavbar,
        CalendarDayView,
        CalendarWeekView,
        CalendarSidebar
    }
})
export default class Calendar extends Vue {
    get view() {
        return calendarStore.view;
    }
}
</script>
