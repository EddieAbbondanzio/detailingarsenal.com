import { PadCategory } from '@/api/product-catalog/data-transfer-objects/pad-category';
import { Brand } from '@/api';
import { PadSeries } from '@/api/product-catalog/data-transfer-objects/pad-series';

export class Pad {
    constructor(
        public id: string,
        public category: PadCategory,
        public series: PadSeries,
        public name: string,
        public color: string,
        public image?: File
    ) {}
}
