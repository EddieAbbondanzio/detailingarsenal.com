import { Brand } from './brand';
import { PadMaterial } from './pad-material';
import { PadTexture } from './pad-texture';
import { PolisherType } from './polisher-type';
import { PadColor } from './pad-color';
import { PadSize } from './pad-size';

export class PadSeries {
    constructor(
        public id: string,
        public name: string,
        public brand: Brand,
        public material: PadMaterial,
        public texture: PadTexture,
        public polisherTypes: PolisherType[],
        public sizes: PadSize[] = [],
        public colors: PadColor[] = []
    ) { }
}
