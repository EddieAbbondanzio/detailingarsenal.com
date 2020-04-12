<template>
    <page background="is-white">
        <template v-slot:header>
            <page-header>
                <calendar-navbar />
            </page-header>
        </template>

        <template v-slot:sidebar>
            <div class="calendar-sidebar has-h-100 has-background-light">
                reee
            </div>
        </template>

        <!-- <div class="is-flex is-flex-row"> -->
        <!-- <div class="has-background-white calendar-sidebar">
                    REEE
                </div> -->

        <calendar-day-view v-if="view == 'day'" />
    </page>
</template>

<style lang="sass" scoped>
.calendar-sidebar
    width: 300px
    border-right: 1px solid $grey-lighter
</style>

<script lang="ts">
import { Component, Vue, Prop, Ref } from 'vue-property-decorator';
import CalendarNavbar from '@/modules/app/components/calendar/calendar-navbar.vue';
import { getModule } from 'vuex-module-decorators';
import CalendarStore from '../../store/calendar/calendar-store';
import CalendarDayView from '@/modules/app/components/calendar/calendar-day-view.vue';

@Component({
    name: 'calendar',
    components: {
        CalendarNavbar,
        CalendarDayView
    }
})
export default class Calendar extends Vue {
    get view() {
        const calendarStore = getModule(CalendarStore, this.$store);
        return calendarStore.view;
    }
}
</script>
