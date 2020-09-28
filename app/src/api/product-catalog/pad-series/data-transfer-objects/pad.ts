import { Brand, PadCategory, Stars } from '@/api';
import { PadSeries } from '@/api/product-catalog/pad-series/data-transfer-objects/pad-series';
import { Image } from '@/api/product-catalog/common/data-transfer-objects/image';
import { PadFinish } from './pad-finish';
import { PadCut } from './pad-cut';
import { PadSize } from './pad-size';
import { PadMaterial } from './pad-material';
import { PadTexture } from './pad-texture';
import { PolisherType } from './polisher-type';
import { Rating } from './rating';

export class Pad {
    get label() {
        return `${this.series.brand.name} ${this.series.name} ${this.name}`
    }

    constructor(
        public id: string,
        public series: PadSeries,
        public name: string,
        public category: PadCategory,
        public cut: PadCut | null,
        public finish: PadFinish | null,
        public material: PadMaterial,
        public texture: PadTexture,
        public sizes: PadSize[],
        public polisherTypes: PolisherType[],
        public rating: Rating,
        public image: Image | null = null,
    ) { }
}
