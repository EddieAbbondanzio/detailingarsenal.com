import 'jest';
import { fullDate } from '@/core/filters/full-date';

describe('fullDate()', () => {
    it('returns an empty string when null', () => {
        expect(fullDate(null!)).toBe('');
    });

    it('properly formats a date', () => {
        expect(fullDate(new Date('02/21/2020'))).toBe('Friday, February 21st 2020');
    });
});
