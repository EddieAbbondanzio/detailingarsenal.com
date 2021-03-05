import { Measurement } from '@/api/shared';

export function measurement(val: Measurement | null): string {
    if (val == null) {
        return '';
    }

    return `${val.amount}${val.unit}`;
}
