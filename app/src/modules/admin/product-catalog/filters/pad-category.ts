import { PadCategory } from '@/api';

export function padCategory(val: number) {
    const output = [];

    if (val & PadCategory.Cutting) {
        output.push('Cutting');
    }

    if (val & PadCategory.Polishing) {
        output.push('Polishing');
    }

    if (val & PadCategory.Finishing) {
        output.push('Finishing');
    }

    return output.join(', ');
}
