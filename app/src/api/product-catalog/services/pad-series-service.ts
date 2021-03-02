import { Brand } from '../data-transfer-objects/brand';
import { PadSeries } from '../data-transfer-objects/pad-series';
import { http } from '@/api/core/http';
import { PadSize } from '../data-transfer-objects/pad-size';
import { Pad } from '../data-transfer-objects/pad';
import { PadSeriesCreateRequest } from '../data-transfer-objects/requests/pad-series-create-request';
import { PadSeriesUpdateRequest } from '../data-transfer-objects/requests/pad-series-update-request';
import { Rating } from '../data-transfer-objects/rating';
import { Image } from '../data-transfer-objects/image';
import { PadOption } from '../data-transfer-objects/pad-option';
import { Measurement } from '../data-transfer-objects/measurement';
import { PadFilterLegend } from '../data-transfer-objects/pad-filter-legend';
import { PagedArray } from '@/api/core/data-transfer-objects/paged-array';
import { PadSeriesGetAllRequest } from '../data-transfer-objects/requests/pad-series-get-all-request';

export class PadSeriesService {
    async get(filter?: PadSeriesGetAllRequest): Promise<PagedArray<PadSeries>> {
        // TODO: Switch to query string?
        const res =
            filter != null
                ? await http.post('product-catalog/pads/filter', filter)
                : await http.get('product-catalog/pad-series');

        return {
            paging: {
                pageCount: res.data.paging.pageCount,
                pageNumber: res.data.paging.pageNumber ?? 0,
                pageSize: res.data.paging.pageSize,
                total: res.data.paging.total
            },
            values: ((res.data.values as any[]) ?? []).map(d => this._map(d))
        };
    }

    async getById(id: string): Promise<PagedArray<PadSeries>> {
        const res = await http.get(`product-catalog/pad-series/${id}`);

        if (res.data != null) {
            const s = this._map(res.data);

            return {
                paging: {
                    total: 1,
                    pageNumber: 1,
                    pageSize: 1,
                    pageCount: 1
                },
                values: [s]
            };
        } else {
            return {
                paging: {
                    total: 1,
                    pageNumber: 1,
                    pageSize: 1,
                    pageCount: 1
                },
                values: []
            };
        }
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
            ps.polisherTypes,
            (ps.sizes ?? ([] as any[])).map(
                (s: any) =>
                    new PadSize(
                        s.id,
                        { amount: s.diameter.amount, unit: s.diameter.unit },
                        s.thickness != null
                            ? { amount: s.thickness.amount, unit: s.thickness.unit }
                            : ((null as any) as Measurement)
                    )
            ),
            (ps.pads ?? ([] as any[])).map(
                (c: any) =>
                    new Pad(
                        c.id,
                        ps,
                        c.name,
                        c.category,
                        c.material,
                        c.texture,
                        c.color,
                        c.hasCenterHole,
                        c.cut,
                        c.finish,
                        new Rating(c.rating.stars ?? 0, c.rating.reviewCount ?? 0, c.rating.stats ?? []),
                        c.imageId,
                        (c.options ?? ([] as any[])).map((o: any) => ({
                            id: o.id,
                            padSizeId: o.padSizeId,
                            partNumbers: o.partNumbers
                        }))
                    )
            )
        );

        return series;
    }
}
