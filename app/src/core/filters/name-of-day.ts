import moment from 'moment';

/**
 * Display the name of a day from it's date.
 * @param date The date to get the day name from.
 */
export function nameOfDay(date: Date) {
    if (date == null) {
        return '';
    }

    const m = moment(date);
    return m.format('dddd');
}
