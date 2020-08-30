import { Brand, PadCategory } from '@/api';
import { PadSeries } from '@/api/product-catalog/pad-series/data-transfer-objects/pad-series';
import { Image } from '@/api/product-catalog/common/data-transfer-objects/image';
import { PadFinish } from './pad-finish';
import { PadCut } from './pad-cut';
import { PadSize } from '../pad-size';
import { PadMaterial } from './pad-material';
import { PadTexture } from './pad-texture';
import { PolisherType } from './polisher-type';

export class Pad {
    constructor(
        public id: string,
        public series: PadSeries,
        public name: string,
        public category: PadCategory,
        public cut: PadCut,
        public finish: PadFinish,
        public material: PadMaterial,
        public texture: PadTexture,
        public sizes: PadSize[],
        public recommendedFor: PolisherType[],
        public image?: Image,
    ) { }
}
