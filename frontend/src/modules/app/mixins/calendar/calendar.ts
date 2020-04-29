import Component from 'vue-class-component';
import Vue from 'vue';
import { AppointmentBlock, BLOCK_MODIFY_FLAG } from '@/modules/app/api/calendar/entities/appointment-block';
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

    get modifyingBlock() {
        return calendarStore.modifyingBlock;
    }

    // remove
    addModifyingFlag(block: AppointmentBlock) {
        calendarStore.ADD_BLOCK_META({ block, meta: { name: BLOCK_MODIFY_FLAG, value: true } });
    }

    // remove
    removeModifyingFlag(block: AppointmentBlock) {
        calendarStore.REMOVE_BLOCK_META({ block, name: BLOCK_MODIFY_FLAG });
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

    //remove?
    deleteBlock(block: AppointmentBlock) {
        calendarStore.DELETE_BLOCK(block);
    }

    //remove?
    /**
     * Save the pending changes on a block to it's new position,
     * or size.
     * @param block The block to apply modifications to.
     */
    saveBlockChanges(block: AppointmentBlock) {
        return calendarStore.saveBlockChanges(block);
    }

    async loadAppointments(date: Date, range: CalendarRange) {
        await calendarStore.loadAppointments({ date, range });
    }
}
