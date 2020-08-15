import { HoursOfOperationDay } from '@/api/scheduling/settings/data-transfer-objects/hours-of-operation-day';

/**
 * Hours of operation that the business is open for.
 */
export class HoursOfOperation {
    /**
     * Create a new set of hours of operation.
     * @param days The open and close time of each day.
     */
    constructor(public id: string, public days: HoursOfOperationDay[] = []) {}

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
