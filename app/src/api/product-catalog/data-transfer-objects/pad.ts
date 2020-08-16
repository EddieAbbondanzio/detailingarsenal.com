import { PadCategory } from '@/api/product-catalog/data-transfer-objects/pad-category';

export class Pad {
    constructor(
        public id: string,
        public category: PadCategory,
        public brand: string,
        public series: string,
        public color: string,
        public image?: File
    ) {}
}
