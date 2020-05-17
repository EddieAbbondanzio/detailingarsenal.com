import 'jest';
import { duration } from '@/core/filters/duration';

describe('duration()', () => {
    it('returns a minute value for durations less than an hour', () => {
        expect(duration(30)).toBe('30m');
    });

    it('returns an hour when no minutes', () => {
        expect(duration(60)).toBe('1h');
    });

    it('returns hours and minutes when needed', () => {
        expect(duration(90)).toBe('1h 30m');
    });
});
