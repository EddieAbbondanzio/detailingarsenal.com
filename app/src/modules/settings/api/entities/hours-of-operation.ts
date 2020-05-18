import { HoursOfOperationDay } from '@/modules/settings/api/entities/hours-of-operation-day';
import { Entity } from '@/core';

/**
 * Hours of operation that the business is open for.
 */
export class HoursOfOperation extends Entity {
    /**
     * Create a new set of hours of operation.
     * @param days The open and close time of each day.
     */
    constructor(public days: HoursOfOperationDay[] = []) {
        super();
    }

    contains(day: Date, time: number) {
        const dayHoursOfOp = this.days.find(d => d.day == day.getDay());

        if (dayHoursOfOp == null) {
            return false;
        }

        return dayHoursOfOp.containsTime(time);
    }

    getEarliestOpening() {
        return Math.min(...this.days.map(d => d.open));
    }

    getHoursForDay(day: number) {
        return this.days.find(d => d.day == day);
    }
}
