import 'jest';
import { twelveHourFormat } from '@/core/filters/twelve-hour-format';

describe('twelveHourFormat()', () => {
    it('returns an empty string when null', () => {
        expect(twelveHourFormat(null!)).toBe('');
    });

    it('returns properly for an integer', () => {
        expect(twelveHourFormat(420)).toBe('7:00 am');
    });

    it('returns properly for a date', () => {
        expect(twelveHourFormat(new Date('04/20/1969'))).toBe('12:00 am');
    });

    it('returns properly for an object', () => {
        expect(twelveHourFormat({ hours: 4, minutes: 20 })).toBe('4:20 am');
    });
});
