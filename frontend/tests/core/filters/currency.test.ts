import 'jest';
import { currency } from '@/core/filters/currency';

describe('currency()', () => {
    it('returns an empty string on null', () => {
        expect(currency(null!)).toBe('');
    });

    it('returns a $USD amount', () => {
        expect(currency(1.99)).toBe('$1.99');
    });

    it('adds commas as needed', () => {
        expect(currency(1234)).toBe('$1,234.00');
    });
});
