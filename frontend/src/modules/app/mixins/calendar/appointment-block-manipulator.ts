import Component from 'vue-class-component';
import Vue from 'vue';
import { AppointmentBlock } from '@/modules/app/api/calendar/entities/appointment-block';
import calendarStore from '@/modules/app/store/calendar/calendar-store';

export const BLOCK_MODIFY_FLAG = 'modifying';
export const BLOCK_MOUSE_OFFSET = 'mouseOffset';
export const BLOCK_INITIAL_TIME = 'initialTime';

@Component
export default class AppointmentBlockManipulator extends Vue {
    addModifyingFlag(block: AppointmentBlock) {
        calendarStore.ADD_BLOCK_META({ block, meta: { name: BLOCK_MODIFY_FLAG, value: true } });
    }

    removeModifyingFlag(block: AppointmentBlock) {
        calendarStore.REMOVE_BLOCK_META({ block, name: BLOCK_MODIFY_FLAG });
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

        calendarStore.RESIZE_BLOCK({
            block,
            time: startTime,
            duration: Math.max(duration, 15)
        });
    }

    /**
     * Start moving a block to a new time.
     * @param block The block to move.
     * @param time The new start time of the block.
     */
    moveBlock(block: AppointmentBlock, time: number) {
        // Check if we need to set an offset to handle the mouse being starting in a different interval than the block
        if (block.meta.mouseOffset == null) {
            calendarStore.ADD_BLOCK_META({
                block,
                meta: { name: BLOCK_MOUSE_OFFSET, value: time - block.time }
            });
        }

        calendarStore.MOVE_BLOCK({
            block,
            time: time - block.meta.mouseOffset
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
}
