import 'jest';
import { date } from '@/core/filters/date';

describe('date()', () => {
    it('returns an empty string on null', () => {
        expect(date(null!)).toBe('');
    });

    it('returns a date in format MM/DD/YYYY', () => {
        expect(date(new Date('04/20/1960'))).toBe('04/20/1960');
    });
});
