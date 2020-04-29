import Component from 'vue-class-component';
import Vue from 'vue';
import { AppointmentBlock } from '@/modules/app/api/calendar/entities/appointment-block';
import calendarStore from '@/modules/app/store/calendar/calendar-store';
import moment from 'moment';
import { CreateAppointmentBlock } from '@/modules/app/api';
import { CalendarRange } from '@/modules/app/store/calendar/calendar-range';

/**
 * Mixin to create, resize, move, or update appointment blocks using
 * minute from midnight offsets.
 */
@Component
export default class Calendar extends Vue {
    /**
     * Current date being viewed
     */
    get date() {
        return calendarStore.date;
    }

    get pendingBlocks() {
        return calendarStore.pendingBlocks;
    }

    createBlock(date: Date, time: number, duration = 30) {
        const start = moment(date)
            .add(time, 'minutes')
            .toDate();

        const end = moment(date)
            .add(time + duration, 'minutes')
            .toDate();

        const block = new AppointmentBlock(start, end, { pending: true, modifying: true });
        block.meta.initialTime = time;
        calendarStore.ADD_BLOCK(block);
        return block;
    }

    async loadAppointments(date: Date, range: CalendarRange) {
        await calendarStore.loadAppointments({ date, range });
    }
}
