import { Pad } from '@/api/product-catalog/data-transfer-objects/pad';

export class PadService {
    async getPads(): Promise<Pad[]> {
        return [
            new Pad('1', 'heavy_cut', 'Rupes', 'Ravioli', 'Red', undefined),
            new Pad('2', 'heavy_cut', 'Griots', 'Ginger', 'Grey', undefined),
            new Pad('3', 'finishing', 'Silky', 'Soft', 'White', undefined),
            new Pad('4', 'medium_cut', 'Mild', 'Sand', 'Tan', undefined),
            new Pad('5', 'medium_polish', 'Cat', 'Dog', 'Black', undefined)
        ];
    }
}
