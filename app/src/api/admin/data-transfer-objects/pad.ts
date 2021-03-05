import { PadFinish } from '../../product-catalog/data-transfer-objects/pad-finish';
import { PadCut } from '../../product-catalog/data-transfer-objects/pad-cut';
import { Rating } from '../../product-catalog/data-transfer-objects/rating';
import { Image } from '../../shared/data-transfer-objects/image';
import { PadOption } from './pad-option';
import { PadTexture } from '../../shared/data-transfer-objects/pad-texture';
import { PadColor } from '../../shared/data-transfer-objects/pad-color';
import { PadSeries } from '..';
import { PadCategory, PadMaterial } from '@/api/shared';
import { imageUrl, thumbnailUrl } from '@/api/shared/utils/image-utils';

export class Pad {
    get imageUrl() {
        return imageUrl(this.imageId);
    }

    get thumbnailUrl() {
        return thumbnailUrl(this.imageId);
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
        public imageId: string | null = null,
        public options: PadOption[] = []
    ) {}
}
