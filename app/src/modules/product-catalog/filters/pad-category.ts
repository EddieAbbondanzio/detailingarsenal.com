import { PadCategory } from '@/api';

export function padCategory(category: PadCategory) {
    switch (category) {
        case PadCategory.Cut:
            return 'Cut';
        case PadCategory.Finishing:
            return 'Finishing';
        case PadCategory.Polish:
            return 'Polish';
        default:
            return '';
    }
}
