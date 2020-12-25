import { Brand } from '../data-transfer-objects/brand';
import { PadSeries } from '../data-transfer-objects/pad-series';
import { http } from '@/api/core/http';
import { PadSize } from '../data-transfer-objects/pad-size';
import { PadColor } from '../data-transfer-objects/pad-color';
import { PadSeriesCreateRequest } from '../data-transfer-objects/requests/pad-series-create-request';
import { PadSeriesUpdateRequest } from '../data-transfer-objects/requests/pad-series-update-request';
import { Rating } from '../data-transfer-objects/rating';
import { Image } from '../data-transfer-objects/image';
import { PadOption } from '../data-transfer-objects/pad-option';

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
        const series = new PadSeries(
            ps.id,
            ps.name,
            new Brand(ps.brand.id, ps.brand.name),
            ps.material,
            ps.texture,
            ps.polisherTypes,
            (ps.sizes ?? [] as any[]).map((s: any) => ({
                id: s.id,
                diameter: { amount: s.diameter.amount, unit: s.diameter.unit },
                thickness: s.thickness != null ? { amount: s.thickness.amount, unit: s.thickness.unit } : null
            })),
            (ps.colors ?? [] as any[]).map((c: any) => new PadColor(
                c.id,
                ps,
                c.name,
                c.category,
                c.cut,
                c.finish,
                new Rating(c.stars, c.reviewCount), c.image != null ? new Image(c.image.name, c.image.data) : null,
                (c.options ?? [] as any[]).map((o: any) => ({
                    padSizeId: o.padSizeId,
                    partNumber: o.partNumber
                })))
            )
        );

        return series;
    }
}
