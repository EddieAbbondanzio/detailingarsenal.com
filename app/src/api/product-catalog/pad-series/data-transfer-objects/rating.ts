import { Stars } from '../../common/data-transfer-objects/stars';

export class Rating {
    constructor(public stars: Stars, public reviewCount: number) {}
}