import { Appointment } from '@/api/scheduling/calendar/data-transfer-objects/appointment';
import moment from 'moment';

export const BLOCK_INITIAL_TIME = 'initialTime';
export const BLOCK_PENDING_FLAG = 'pending';
export const BLOCK_MODIFIED = 'modified';

export class AppointmentBlock {
    appointment: Appointment = null!;

    constructor(public id: string, public start: Date, public end: Date, public meta: any = {}) {}

    get time() {
        const startM = moment(this.start);
        const endM = moment(this.start).startOf('day');
        return moment.duration(startM.diff(endM)).asMinutes();
    }

    get duration() {
        return moment.duration(moment(this.end).diff(moment(this.start))).asMinutes();
    }
}
