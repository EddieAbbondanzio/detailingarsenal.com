import moment from 'moment';

/**
 * Display a time value as 12:00 am format.
 * 4 -> 5:00 am
 * {hours: 2, minutes: 15} -> 3:15 am
 * @param val Numeric value of 0 - 23, or {} with hours, and minutes on it.
 */
export function twelveHourFormat(val: number | { hours: number; minutes: number } | Date) {
    if (val == null) {
        return '';
    }

    if (typeof val == 'number') {
        const m = moment.utc(val * 1000 * 60);
        return m.format('h:mm a');
    } else if (val instanceof Date) {
        const m = moment(val);
        return m.format('h:mm a');
    } else {
        const suffix = val.hours >= 12 ? 'pm' : 'am';
        const hours = ((val.hours + 11) % 12) + 1;
        const minutes = val.minutes < 10 ? '0' + val.minutes.toString() : val.minutes.toString();

        return `${hours}:${minutes} ${suffix}`;
    }
}
