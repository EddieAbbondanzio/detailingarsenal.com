import 'jest';
import { nameOfDay } from '@/core/filters/name-of-day';

describe('nameOfDay()', () => {
    it('returns an empty string if null', () => {
        expect(nameOfDay(null!)).toBe('');
    });

    it('returns the day name', () => {
        expect(nameOfDay(new Date('02/21/2020'))).toBe('Friday');
    });
});
