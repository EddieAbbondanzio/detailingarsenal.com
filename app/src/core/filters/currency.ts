/**
 * Filter to display a decimal as a USD value.
 * Format: $1.99
 */
export function currency(curr: number) {
    if (curr == null) {
        return '';
    }

    const formatter = new Intl.NumberFormat('en-US', {
        style: 'currency',
        currency: 'USD',
        minimumFractionDigits: 2
    });

    return formatter.format(curr);
}
