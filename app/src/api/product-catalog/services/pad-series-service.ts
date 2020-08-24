import { Pad } from '@/api/product-catalog/data-transfer-objects/pad';
import { PadSeriesCreate } from '@/api/product-catalog/data-transfer-objects/pad-series-create';
import { PadSeriesUpdate } from '@/api/product-catalog/data-transfer-objects/pad-series-update';
import { PadSeries, Brand } from '@/api';
import { http } from '@/api/core/http';

export class PadSeriesService {
    async get(): Promise<PadSeries[]> {
        return [];
    }

    async create(create: PadSeriesCreate) {
        const res = await http.post('product-catalog/pad-series', create);
        const ps = this._map(res.data);
        return ps;
    }

    async update(update: PadSeriesUpdate) {
        const res = await http.put(`product-catalog/pad-series/${update.id}`, update);
        const ps = this._map(res.data);
        return ps;
    }

    async delete(id: string) {
        await http.delete(`product-catalog/pad-series/${id}`);
    }

    _map(ps: any): PadSeries {
        const series = new PadSeries(ps.id, ps.name, new Brand(ps.brand.id, ps.brand.name));

        series.pads = (ps.pads as any[]).map(p => new Pad(p.id, p.category, series, p.name, p.image));

        return series;
    }
}
