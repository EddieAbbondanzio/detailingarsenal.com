import { Entity } from '@/core';
import { Appointment } from '@/modules/app/api/calendar/entities/appointment';
import moment from 'moment';

export const BLOCK_MODIFY_FLAG = 'modifying';
export const BLOCK_MOUSE_OFFSET = 'mouseOffset';
export const BLOCK_INITIAL_TIME = 'initialTime';
export const BLOCK_PENDING_FLAG = 'pending';
export const BLOCK_MODIFIED = 'modified';

export class AppointmentBlock extends Entity {
    appointment: Appointment = null!;

    constructor(public start: Date, public end: Date, public meta: any = {}) {
        super();
    }

    get time() {
        const startM = moment(this.start);
        const endM = moment(this.start).startOf('day');
        return moment.duration(startM.diff(endM)).asMinutes();
    }

    get duration() {
        return moment.duration(moment(this.end).diff(moment(this.start))).asMinutes();
    }
}
