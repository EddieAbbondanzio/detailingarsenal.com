import moment from 'moment';

/**
 * Format a date as MM/DD/YYYY
 * @param date The date to format.
 */
export function date(date: Date) {
    if (date == null) {
        return '';
    }

    const m = moment(date);
    return m.format('MM/DD/YYYY');
}
