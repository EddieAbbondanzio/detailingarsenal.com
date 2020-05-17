import moment from 'moment';
import { DayOfTheWeek } from '@/modules/app/store/day-of-the-week';

export class TimeUtils {
    public static timeToHoursMinutes(time: number) {
        if (time < 0 || time > 1440) {
            throw new RangeError();
        }

        const minutes = time % 60;
        const hours = (time - minutes) / 60;

        return { hours, minutes };
    }

    public static hoursAndMinutesToTime(hours: number, minutes: number) {
        if (hours < 0 || hours > 23 || minutes < 0 || minutes > 59) {
            throw new RangeError();
        }

        return hours * 60 + minutes;
    }

    public static timeToDate(time: number) {
        const { hours, minutes } = this.timeToHoursMinutes(time);

        const d = new Date();
        d.setHours(hours, minutes);

        return d;
    }

    public static dateToTime(date: Date) {
        return date.getHours() * 60 + date.getMinutes();
    }

    public static getFirstDateOfWeek(date: Date | null = null) {
        if (date == null) {
            date = new Date();
        }

        return moment(date)
            .startOf('week')
            .toDate();
    }

    public static getFirstOfMonth(date: Date | null = null) {
        if (date == null) {
            date = new Date();
        }

        return moment(date)
            .startOf('month')
            .toDate();
    }

    public static dayIndexToName(day: number): DayOfTheWeek {
        switch (day) {
            case 0:
                return 'Sunday';
            case 1:
                return 'Monday';
            case 2:
                return 'Tuesday';
            case 3:
                return 'Wednesday';
            case 4:
                return 'Thursday';
            case 5:
                return 'Friday';
            case 6:
                return 'Saturday';
            default:
                throw new Error();
        }
    }

    public static dayNameToIndex(day: DayOfTheWeek) {
        switch (day) {
            case 'Sunday':
                return 0;
            case 'Monday':
                return 1;
            case 'Tuesday':
                return 2;
            case 'Wednesday':
                return 3;
            case 'Thursday':
                return 4;
            case 'Friday':
                return 5;
            case 'Saturday':
                return 6;
        }
    }
}
