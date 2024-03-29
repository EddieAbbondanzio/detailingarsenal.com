import { http } from '@/api/shared/http';
import brandStore from '@/modules/admin/product-catalog/store/brand-store';
import { PadFilterLegend } from '../data-transfer-objects/pad-filter-legend';

export class PadFilterService {
    async get(): Promise<PadFilterLegend> {
        var res = await http.get('product-catalog/pads/filter');
        return res.data;
    }
}

export const padFilterService = new PadFilterService();
