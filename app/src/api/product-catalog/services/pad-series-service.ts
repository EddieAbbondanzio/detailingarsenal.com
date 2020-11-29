import { Brand } from '../data-transfer-objects/brand';
import { PadSeries } from '../data-transfer-objects/pad-series';
import { http } from '@/api/core/http';
import { PadSeriesSize } from '../data-transfer-objects/pad-series-size';
import { Pad } from '../data-transfer-objects/pad';
import { PadSeriesCreateRequest } from '../data-transfer-objects/requests/pad-series-create-request';
import { PadSeriesUpdateRequest } from '../data-transfer-objects/requests/pad-series-update-request';

export class PadSeriesService {
    async get(): Promise<PadSeries[]> {
        const res = await http.get('product-catalog/pad-series');
        return (res.data as any[]).map(d => this._map(d));
    }

    async create(create: PadSeriesCreateRequest) {
        const res = await http.post('product-catalog/pad-series', create);
        const ps = this._map(res.data);
        return ps;
    }

    async update(update: PadSeriesUpdateRequest) {
        const res = await http.put(`product-catalog/pad-series/${update.id}`, update);
        const ps = this._map(res.data);
        return ps;
    }

    async delete(id: string) {
        await http.delete(`product-catalog/pad-series/${id}`);
    }

    _map(ps: any): PadSeries {
        const series = new PadSeries(ps.id, ps.name, new Brand(ps.brand.id, ps.brand.name));

        series.pads = (ps.pads as any[]).map(p => new Pad(p.id, series, p.name, p.category, p.cut, p.finish, p.material, p.texture, p.polisherTypes, null!, p.image));
        series.sizes = (ps.sizes as any[]).map(s => new PadSeriesSize(s.diameter, s.thickness, s.partNumber));

        return series;
    }
}
