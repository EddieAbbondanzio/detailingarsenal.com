import { PadCategory, PadColor, PadMaterial, PadTexture, PolisherType } from '@/api/shared';
import { imageUrl, thumbnailUrl } from '@/api/shared/utils/image-utils';
import { Rating } from './rating';

export class Pad {
    get label() {
        /*
         *  Some brands have pads without a series. Ex: Griots Garage.
         * To compensate for this we create a series with the same name
         * as the brand, and simply hide it.
         */
        if (this.brand.name == this.series.name) {
            return `${this.brand.name} ${this.name}`;
        } else {
            return `${this.brand.name} ${this.series.name} ${this.name}`;
        }
    }

    get imageUrl() {
        return imageUrl(this.imageId);
    }

    get thumbnailUrl() {
        return thumbnailUrl(this.imageId);
    }

    constructor(
        public id: string,
        public name: string,
        public series: { id: string; name: string },
        public brand: { id: string; name: string },
        public category: PadCategory[],
        public color: PadColor,
        public material: PadMaterial,
        public texture: PadTexture,
        public cut: number | null,
        public finish: number | null,
        public rating: Rating,
        public polisherTypes: PolisherType[],
        public hasCenterHole: boolean,
        public imageId: string | null
    ) {}
}
