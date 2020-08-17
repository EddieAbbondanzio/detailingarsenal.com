import { PadCategory } from '@/api';

export function padCategory(category: PadCategory) {
    switch (category) {
        case 'heavy_cut':
            return 'Heavy cut';
        case 'medium_cut':
            return 'Medium cut';
        case 'heavy_polish':
            return 'Heavy polish';
        case 'medium_polish':
            return 'Medium polish';
        case 'soft_polish':
            return 'Soft polish';
        case 'finishing':
            return 'Finishing';
        default:
            return '';
    }
}
