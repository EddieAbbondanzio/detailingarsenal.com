import { Pad, Brand } from '@/api';
import { PadSeriesSize } from './pad-series-size';

export class PadSeries {
    constructor(public id: string, public name: string, public brand: Brand, public sizes: PadSeriesSize[] = [], public pads: Pad[] = []) { }
}
