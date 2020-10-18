import { Pad } from '@/api/product-catalog/pad-series/data-transfer-objects/pad';
import { PadSeriesCreateRequest } from '@/api/product-catalog/pad-series/data-transfer-objects/requests/pad-series-create-request';
import { PadSeriesUpdateRequest } from '@/api/product-catalog/pad-series/data-transfer-objects/requests/pad-series-update-request';
import { PadSeries, Brand } from '@/api';
import { http } from '@/api/core/http';

export class PadSeriesService {
    async get(): Promise<PadSeries[]> {
        const res = await http.get('product-catalog/pad');
        return (res.data as any[]).map(d => this._map(d));
    }

    async create(create: PadSeriesCreateRequest) {
        const res = await http.post('product-catalog/pad', create);
        const ps = this._map(res.data);
        return ps;
    }

    async update(update: PadSeriesUpdateRequest) {
        const res = await http.put(`product-catalog/pad/${update.id}`, update);
        const ps = this._map(res.data);
        return ps;
    }

    async delete(id: string) {
        await http.delete(`product-catalog/pad/${id}`);
    }

    _map(ps: any): PadSeries {
        const series = new PadSeries(ps.id, ps.name, new Brand(ps.brand.id, ps.brand.name));

        // series.pads = (ps.pads as any[]).map(p => new Pad(p.id, p.category, series, p.name, p.image));

        return series;
    }
}
