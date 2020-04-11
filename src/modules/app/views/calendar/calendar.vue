<template>
    <page>
        <template v-slot:header>
            <page-header>
                <calendar-navbar />
            </page-header>
        </template>

        <template v-slot:body>
            <div class="has-background-white">
                <calendar-day-view v-if="view == 'day'" />
            </div>
        </template>
    </page>
</template>

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
