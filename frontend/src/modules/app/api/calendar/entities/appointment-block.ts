import { Entity } from '@/core';
import { Appointment } from '@/modules/app/api/calendar/entities/appointment';

export class AppointmentBlock extends Entity {
    appointment: Appointment = null!;

    /**
     * Create a new appointment time.
     * @param appointment The appointment it belongs to.
     * @param date The date
     * @param time The time of day in minutes from midnight.
     * @param duration How long the appointment is for.
     */
    constructor(public date: Date, public time: number, public duration: number, public meta: any = {}) {
        super();
    }
}
