import { http } from '@/api/core/http';
import brandStore from '@/modules/admin/product-catalog/store/brand-store';
import { PadSeriesFilterLegend } from '../data-transfer-objects/pad-series-filter-legend';

export class PadSeriesFilterService {
    async get(): Promise<PadSeriesFilterLegend> {
        var res = await http.get('product-catalog/pad-series/filter');
        return res.data;
    }
}
