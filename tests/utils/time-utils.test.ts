import 'jest';
import { TimeUtils } from '@/core/utils/time-utils';

describe('TimeUtils', () => {
    describe('timeToHoursMinutes()', () => {
        test('returns 0h 0m for 0', () => {
            expect(TimeUtils.timeToHoursMinutes(0)).toMatchObject({ hours: 0, minutes: 0 });
        });

        test('returns 7h for 420', () => {
            expect(TimeUtils.timeToHoursMinutes(420)).toMatchObject({ hours: 7, minutes: 0 });
        });

        test('returns 13h 30m for 810', () => {
            expect(TimeUtils.timeToHoursMinutes(810)).toMatchObject({ hours: 13, minutes: 30 });
        });

        test('throw RangeError if less than 0', () => {
            expect(() => {
                TimeUtils.timeToHoursMinutes(-1);
            }).toThrowError(RangeError);
        });

        test('throws RangeError if greater than 1440', () => {
            expect(() => {
                TimeUtils.timeToHoursMinutes(1441);
            }).toThrowError(RangeError);
        });
    });

    describe('hoursAndMinutesToTime()', () => {
        test('returns 0 for 0h 0m', () => {});

        test('returns 420 for 7h', () => {});

        test('returns 810 for 13h 30m', () => {});

        test('throws RangeError if hours < 0', () => {
            expect(() => {
                TimeUtils.hoursAndMinutesToTime(-1, 0);
            }).toThrowError(RangeError);
        });

        test('throws RangeError if hours > 23', () => {
            expect(() => {
                TimeUtils.hoursAndMinutesToTime(24, 0);
            }).toThrowError(RangeError);
        });

        test('throws RangeError if minutes < 0', () => {
            expect(() => {
                TimeUtils.hoursAndMinutesToTime(0, -1);
            }).toThrowError(RangeError);
        });

        test('throws RangeError if minutes > 59', () => {
            expect(() => {
                TimeUtils.hoursAndMinutesToTime(0, 60);
            }).toThrowError(RangeError);
        });
    });

    describe('timeToDate()', () => {
        test('returns date with proper hours', () => {
            expect(TimeUtils.timeToDate(570).getHours()).toBe(9);
        });

        test('returns date with proper minutes', () => {
            expect(TimeUtils.timeToDate(570).getMinutes()).toBe(30);
        });
    });

    describe('dateToTime()', () => {
        test('returns time with proper minutes', () => {
            const d = new Date();
            d.setHours(9, 30);
            expect(TimeUtils.dateToTime(d)).toBe(570);
        });
    });

    describe('getFirstDateOfWeek()', () => {
        it('returns the first day of a week', () => {
            const firstDay = TimeUtils.getFirstDateOfWeek(new Date('02/10/20'));
            const actualFirstDay = new Date('02/09/20');

            expect(firstDay.getDate()).toEqual(actualFirstDay.getDate());
        });

        it('returns first day of current week if no date passed', () => {
            const firstDay = TimeUtils.getFirstDateOfWeek();
            const firstDayTest = TimeUtils.getFirstDateOfWeek(new Date());

            expect(firstDay.getDate()).toEqual(firstDayTest.getDate());
        });
    });
});
