import { http } from '@/api/shared';
import { PadSize } from '../data-transfer-objects/pad-size';

export class PadSizeService {
    async getForPad(padId: string): Promise<PadSize[]> {
        var res = await http.get(`product-catalog/pads/${padId}/sizes`);
        return res.data;
    }
}

export const padSizeService = new PadSizeService();
