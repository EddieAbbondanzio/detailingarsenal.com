import { Pad, Brand } from '@/api';

export class PadSeries {
    constructor(public id: string, public name: string, public brand: Brand, public pads: Pad[] = []) {}
}
