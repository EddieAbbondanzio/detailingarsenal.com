<template>
    <div
        class="has-padding-all-3 calendar-sidebar has-background-light is-flex is-flex-column is-align-items-center has-h-100"
    >
        <b-button
            class="has-w-100 has-margin-bottom-3"
            type="is-success"
            icon-left="calendar-plus"
            @click="onCreateClick"
        >Create appointment</b-button>

        <calendar-datepicker class="has-w-100" />
    </div>
</template>

<style lang="sass" scoped>
.calendar-sidebar
    width: 300px
    border-right: 1px solid $grey-lighter
</style>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import CalendarDatepicker from '@/modules/calendar/components/calendar-datepicker.vue';
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