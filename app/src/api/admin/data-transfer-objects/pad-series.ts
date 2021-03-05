import { Brand } from './brand';
import { PolisherType } from '../../shared/data-transfer-objects/polisher-type';
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
