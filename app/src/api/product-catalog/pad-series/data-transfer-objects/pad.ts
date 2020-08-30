import { Brand, PadCategory } from '@/api';
import { PadSeries } from '@/api/product-catalog/pad-series/data-transfer-objects/pad-series';
import { Image } from '@/api/product-catalog/common/data-transfer-objects/image';

export class Pad {
    constructor(
        public id: string,
        public category: PadCategory,
        public series: PadSeries,
        public name: string,
        public image?: Image
    ) { }
}
