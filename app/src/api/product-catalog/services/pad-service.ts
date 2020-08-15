import { Pad } from '@/api/product-catalog/data-transfer-objects/pad';

export class PadService {
    async getPads(): Promise<Pad[]> {
        return [
            new Pad('1', 'heavy_cut', 'Rupes', 'Ravioli', undefined),
            new Pad('1', 'heavy_cut', 'Griots', 'Ginger', undefined),
            new Pad('1', 'finishing', 'Silky', 'Soft', undefined),
            new Pad('1', 'medium_cut', 'Mild', 'Sand', undefined),
            new Pad('1', 'medium_polish', 'Cat', 'Dog', undefined)
        ];
    }
}
