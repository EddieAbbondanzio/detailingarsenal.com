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

    resizeBlock(block: AppointmentBlock, time: number) {
        // Easy peazy. We're resizing an existing block.
        if (block.meta.initialTime == null) {
            calendarStore.RESIZE_BLOCK({
                block,
                time: block.time,
                duration: time - block.time + 15
            });

            return;
        }

        // Down
        if (block.time < time) {
            // Going down, but we went up first
            if (block.meta.initialTime > block.time) {
                calendarStore.RESIZE_BLOCK({
                    block,
                    time: time,
                    duration: block.meta.initialTime - time + 15
                });
            } else {
                calendarStore.RESIZE_BLOCK({
                    block,
                    time: block.meta.initialTime,
                    duration: time - block.meta.initialTime + 15
                });
            }
        }
        // Up
        else {
            calendarStore.RESIZE_BLOCK({
                block,
                time: time,
                duration: block.meta.initialTime - time + 15
            });
        }
    }

    /**
     * Start moving a block to a new time.
     * @param block The block to move.
     * @param time The new time of the block.
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
