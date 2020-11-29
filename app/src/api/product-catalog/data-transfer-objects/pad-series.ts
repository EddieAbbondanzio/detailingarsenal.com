import { Pad } from './pad';
import { Brand } from './brand';
import { PadSeriesSize } from './pad-series-size';

export class PadSeries {
    constructor(public id: string, public name: string, public brand: Brand, public sizes: PadSeriesSize[] = [], public pads: Pad[] = []) { }
}
