import brandStore from '@/modules/admin/product-catalog/store/brand-store';
import { PadSeriesFilter } from '../data-transfer-objects/pad-series-filter';

export class PadSeriesFilterService {
    async get(): Promise<PadSeriesFilter> {
        return {
            brands: [
                { id: '1', name: 'Cat Dog' },
                { id: '2', name: 'Mickey Mouse' },
                { id: '3', name: 'Bort' }
            ],
            paging: { pageSize: 10, pageNumber: 0 }
        };
    }
}
