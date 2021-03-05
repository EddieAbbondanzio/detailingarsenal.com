import { PadFinish } from '../../product-catalog/data-transfer-objects/pad-finish';
import { PadCut } from '../../product-catalog/data-transfer-objects/pad-cut';
import { Rating } from '../../product-catalog/data-transfer-objects/rating';
import { Image } from '../../shared/data-transfer-objects/image';
import { PadOption } from './pad-option';
import { PadTexture } from '../../shared/data-transfer-objects/pad-texture';
import { PadColor } from '../../shared/data-transfer-objects/pad-color';
import { PadSeries } from '..';
import { PadCategory, PadMaterial } from '@/api/shared';

export class Pad {
    get imageUrl() {
        return this.image != null ? `${process.env.VUE_APP_API_DOMAIN}/images/${this.image}` : null;
    }

    get thumbnailUrl() {
        return this.image != null ? `${process.env.VUE_APP_API_DOMAIN}/images/${this.image}/thumbnail` : null;
    }

    constructor(
        public id: string,
        public name: string,
        public category: PadCategory[],
        public material: PadMaterial | null,
        public texture: PadTexture | null,
        public color: PadColor | null,
        public hasCenterHole: boolean | null,
        public cut: number | null, // Comes from review
        public finish: number | null, // Comes from review
        public rating: Rating,
        public image: string | null = null,
        public options: PadOption[] = []
    ) {}
}
