import { Brand, PadCategory, PadSeries, Stars } from '@/api';
import { PadFinish } from './pad-finish';
import { PadCut } from './pad-cut';
import { Rating } from './rating';
import { Image } from './image';
import { PadOption } from './pad-option';

export class PadColor {
    get label() {
        return `${this.series.brand.name} ${this.series.name} ${this.name}`
    }

    constructor(
        public id: string,
        public series: PadSeries, // Used for quick lookup of series info only
        public name: string,
        public category: PadCategory,
        public cut: PadCut | null, // Comes from review
        public finish: PadFinish | null, // Comes from review
        public rating: Rating,
        public image: Image | null = null,
        public options: PadOption[] = []
    ) { }
}
