<template>
    <page-sidebar>
        <b-button
            class="has-w-100 has-margin-bottom-3"
            type="is-success"
            icon-left="calendar-plus"
            @click="onCreateClick"
        >Create appointment</b-button>

        <calendar-datepicker class="has-w-100" />
    </page-sidebar>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import CalendarDatepicker from '@/modules/scheduling/calendar/components/calendar-datepicker.vue';
import calendarStore from '../store/calendar-store';

@Component({
    name: 'calendar-sidebar',
    components: {
        CalendarDatepicker
    }
})
export default class CalendarSidebar extends Vue {
    onCreateClick() {
        calendarStore.SET_CREATE_STEP('details');
    }
}
</script>