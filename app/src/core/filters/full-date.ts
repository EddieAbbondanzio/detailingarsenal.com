import moment from 'moment';

/**
 * Format a date as DAYNAME, MONTH DAY YEAR
 * @param date The date to format
 */
export function fullDate(date: Date) {
    if (date == null) {
        return '';
    }

    const m = moment(date);
    return m.format('dddd, MMMM Do YYYY');
}
