import Component from 'vue-class-component';
import Vue from 'vue';
import { AppointmentBlock } from '@/modules/app/api/calendar/entities/appointment-block';
import calendarStore from '@/modules/app/store/calendar/calendar-store';
import moment from 'moment';
import { CreateAppointmentBlock } from '@/modules/app/api';

export const BLOCK_MODIFY_FLAG = 'modifying';
export const BLOCK_MOUSE_OFFSET = 'mouseOffset';
export const BLOCK_INITIAL_TIME = 'initialTime';

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

    getBlock(time: number) {
        const minutes = time % 60;
        const hours = (time - minutes) / 60;

        const start = moment(this.date)
            .hours(hours)
            .minutes(minutes);

        return calendarStore.pendingBlocks.find(b => moment(b.start).isSame(start, 'minutes'));
    }

    addModifyingFlag(block: AppointmentBlock) {
        calendarStore.ADD_BLOCK_META({ block, meta: { name: BLOCK_MODIFY_FLAG, value: true } });
    }

    removeModifyingFlag(block: AppointmentBlock) {
        calendarStore.REMOVE_BLOCK_META({ block, name: BLOCK_MODIFY_FLAG });
    }

    createBlock(date: Date, time: number, duration = 30) {
        const end = moment(date)
            .add(time + duration, 'minutes')
            .toDate();

        const block = new AppointmentBlock(date, end, { pending: true, modifying: true });
        block.meta.initialTime = time;
        calendarStore.ADD_BLOCK(block);
        return block;
    }

    updateBlockStart(block: AppointmentBlock, date: Date) {
        calendarStore.UPDATE_BLOCK_START({ block, start: date });
    }

    updateBlockEnd(block: AppointmentBlock, date: Date) {
        calendarStore.UPDATE_BLOCK_END({ block, end: date });
    }

    deleteBlock(block: AppointmentBlock) {
        calendarStore.DELETE_BLOCK(block);
    }

    /**
     * Resize an existing block.
     * @param block The block to adjust.
     * @param endTime The new end time of the block.
     */
    resizeBlock(block: AppointmentBlock, endTime: number) {
        let startTime = 0,
            duration = 0;

        // Easy peazy. We're resizing an existing block.
        if (block.meta.initialTime == null) {
            startTime = block.time;
            duration = endTime - block.time;
        }
        // Down
        else if (block.time < endTime) {
            // Going down, but we went up first
            if (block.meta.initialTime > block.time) {
                startTime = endTime;
                duration = block.meta.initialTime - endTime + 15;
            } else {
                startTime = block.meta.initialTime;
                duration = endTime - block.meta.initialTime + 15;
            }
        }
        // Up
        else {
            startTime = endTime;
            duration = block.meta.initialTime - endTime + 15;
        }

        const start = moment(block.start)
            .startOf('day')
            .add(startTime, 'minutes')
            .toDate();
        const end = moment(block.start)
            .startOf('day')
            .add(startTime + Math.max(duration, 15), 'minutes')
            .toDate();

        calendarStore.UPDATE_BLOCK_START({
            block,
            start
        });

        calendarStore.UPDATE_BLOCK_END({
            block,
            end
        });
    }

    /**
     * Start moving a block to a new time.
     * @param block The block to move.
     * @param startTime The new start time of the block.
     */
    moveBlock(block: AppointmentBlock, startTime: number) {
        // Check if we need to set an offset to handle the mouse being starting in a different interval than the block
        if (block.meta.mouseOffset == null) {
            calendarStore.ADD_BLOCK_META({
                block,
                meta: { name: BLOCK_MOUSE_OFFSET, value: startTime - block.time }
            });
        }

        const duration = block.duration;

        const start = moment(block.start)
            .startOf('day')
            .add(startTime - block.meta.mouseOffset, 'minutes');

        calendarStore.UPDATE_BLOCK_START({
            block,
            start: start.toDate()
        });

        const end = start.add(duration, 'minutes');

        calendarStore.UPDATE_BLOCK_END({
            block,
            end: end.toDate()
        });
    }

    /**
     * Save the pending changes on a block to it's new position,
     * or size.
     * @param block The block to apply modifications to.
     */
    saveBlockChanges(block: AppointmentBlock) {
        calendarStore.REMOVE_BLOCK_META({
            block,
            name: BLOCK_MOUSE_OFFSET
        });

        calendarStore.REMOVE_BLOCK_META({
            block,
            name: BLOCK_MODIFY_FLAG
        });

        calendarStore.REMOVE_BLOCK_META({
            block,
            name: BLOCK_INITIAL_TIME
        });
    }

    async createAppointment(
        serviceId: string,
        price: number,
        clientId: string,
        blocks: CreateAppointmentBlock[],
        notes: string
    ) {
        return calendarStore.createAppointment({
            serviceId,
            price,
            clientId,
            blocks,
            notes
        });
    }
}
