import { AppointmentBlock } from '@/modules/app/api/calendar/entities/appointment-block';
import { Entity } from '@/core';
import moment from 'moment';

export class Appointment extends Entity {
    public static NOTES_MAX_LENGTH: number = 1024;

    /**
     * Create a new appointment.
     * @param serviceId The id of the service being performed.
     * @param price The price in dollars.
     * @param duration The minutes it will take.
     * @param blocks The times the appointment is on.
     * @param notes Misc. notes.
     */
    constructor(
        public serviceId: string,
        public clientId: string,
        public price: number,
        public blocks: AppointmentBlock[],
        public notes?: string
    ) {
        super();

        if (this.notes != null && this.notes.length > Appointment.NOTES_MAX_LENGTH) {
            throw new RangeError();
        }
    }

    get duration(): number {
        return this.blocks.map(b => b.duration).reduce((a, b) => a + b, 0);
    }

    getBlocksForDay(date: Date): AppointmentBlock[] {
        const dayM = moment(date);

        return this.blocks.filter(b => {
            const blockM = moment(b.date);
            return blockM.isSame(dayM, 'day');
        });
    }

    getBlocksForWeek(date: Date): AppointmentBlock[] {
        const weekM = moment(date);

        return this.blocks.filter(b => {
            const blockM = moment(b.date);
            return blockM.isSame(weekM, 'week');
        });
    }
}
