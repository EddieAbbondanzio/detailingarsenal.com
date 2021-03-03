import { Brand } from './brand';
import { PadMaterial } from './pad-material';
import { PadTexture } from './pad-texture';
import { PolisherType } from './polisher-type';
import { Pad } from './pad';
import { PadSize } from './pad-size';

export class PadSeries {
    constructor(
        public id: string,
        public name: string,
        public brand: Brand,
        public polisherTypes: PolisherType[],
        public sizes: PadSize[] = [],
        public pads: Pad[] = []
    ) {}
}
