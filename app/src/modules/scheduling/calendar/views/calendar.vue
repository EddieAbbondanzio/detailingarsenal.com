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

        <calendar-day v-if="view == 'day'" />
        <calendar-week v-if="view == 'week'" />

        <template v-slot:overlays>
            <time-selection-snackbar v-if="hasPendingBlocks" />
            <appointment-create-modal />
            <appointment-details-modal />
        </template>
    </page>
</template>

<style lang="sass" scoped>

</style>

<script lang="ts">
import { Component, Vue, Prop, Ref } from 'vue-property-decorator';
import CalendarNavbar from '@/modules/scheduling/calendar/components/calendar-navbar.vue';
import CalendarDay from '@/modules/scheduling/calendar/components/calendar-day.vue';
import CalendarWeek from '@/modules/scheduling/calendar/components/calendar-week.vue';
import CalendarSidebar from '@/modules/scheduling/calendar/components/calendar-sidebar.vue';
import TimeSelectionSnackbar from '@/modules/scheduling/calendar/components/overlays/time-selection-snackbar.vue';
import AppointmentCreateModal from '@/modules/scheduling/calendar/components/overlays/appointment-create-modal.vue';
import AppointmentDetailsModal from '@/modules/scheduling/calendar/components/overlays/appointment-details-modal.vue';
import calendarStore from '../store/calendar-store';

@Component({
    name: 'calendar',
    components: {
        CalendarNavbar,
        CalendarDay,
        CalendarWeek,
        CalendarSidebar,
        TimeSelectionSnackbar,
        AppointmentCreateModal,
        AppointmentDetailsModal
    }
})
export default class Calendar extends Vue {
    get view() {
        return calendarStore.view;
    }

    get hasPendingBlocks() {
        return calendarStore.pendingBlocks.length > 0;
    }

    beforeDestroy() {
        calendarStore.CLEAR_BLOCKS();
        calendarStore.CLEAR_CREATE_STEP();
    }
}
</script>
