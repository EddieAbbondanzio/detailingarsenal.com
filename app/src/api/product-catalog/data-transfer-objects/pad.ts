import { Brand, PadCategory, PadSeries, Stars } from '@/api';
import { PadFinish } from './pad-finish';
import { PadCut } from './pad-cut';
import { Rating } from './rating';
import { Image } from './image';
import { PadOption } from './pad-option';
import { PadMaterial } from './pad-material';
import { PadTexture } from './pad-texture';
import { PadColor } from './pad-color';

export class Pad {
    get label() {
        return `${this.series.brand.name} ${this.series.name} ${this.name}`;
    }

    get imageUrl() {
        return this.image != null ? `${process.env.VUE_APP_API_DOMAIN}/image/${this.image}` : null;
    }

    get thumbnailUrl() {
        return this.image != null ? `${process.env.VUE_APP_API_DOMAIN}/image/${this.image}/thumb` : null;
    }

    constructor(
        public id: string,
        public series: PadSeries, // Used for quick lookup of series info only
        public name: string,
        public category: PadCategory,
        public material: PadMaterial | null,
        public texture: PadTexture | null,
        public color: PadColor | null,
        public cut: number | null, // Comes from review
        public finish: number | null, // Comes from review
        public rating: Rating,
        public image: string | null = null,
        public options: PadOption[] = []
    ) {}
}