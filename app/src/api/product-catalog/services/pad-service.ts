import { Pad } from '@/api/product-catalog/data-transfer-objects/pad';

export class PadService {
    async getPads(): Promise<Pad[]> {
        return [
            new Pad('1', 'heavy_cut', 'Rupes', 'Ravioli', 'Sauce', 'Red', undefined),
            new Pad('2', 'heavy_cut', 'Griots', 'Ginger', 'Ghost', 'Grey', undefined),
            new Pad('3', 'finishing', 'Silky', 'Soft', 'Salad', 'White', undefined)
        ];
    }
}
