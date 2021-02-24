import { http } from '@/api/core/http';
import brandStore from '@/modules/admin/product-catalog/store/brand-store';
import { PadSeriesFilter } from '../data-transfer-objects/pad-series-filter';

export class PadSeriesFilterService {
    async get(): Promise<PadSeriesFilter> {
        var res = await http.get('product-catalog/pad-series/filter');
        return res.data;
    }
}
