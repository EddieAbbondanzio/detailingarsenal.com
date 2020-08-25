import { PadCategory } from '@/api/product-catalog/data-transfer-objects/pad-category';
import { Brand } from '@/api';
import { PadSeries } from '@/api/product-catalog/data-transfer-objects/pad-series';
import { Image } from '@/api/product-catalog/data-transfer-objects/image';

export class Pad {
    constructor(
        public id: string,
        public category: PadCategory,
        public series: PadSeries,
        public name: string,
        public image?: Image
    ) {}
}