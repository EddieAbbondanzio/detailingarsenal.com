import { PadCategory } from '@/api';

export function padCategory(category: PadCategory) {
    switch (category) {
        case PadCategory.Cutting:
            return 'Cutting';
        case PadCategory.Finishing:
            return 'Finishing';
        case PadCategory.Polishing:
            return 'Polishing';
        default:
            return '';
    }
}
