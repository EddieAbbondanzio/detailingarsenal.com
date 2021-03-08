import { Brand } from '../data-transfer-objects/brand';
import { PadSeries } from '../data-transfer-objects/pad-series';
import { http } from '@/api/shared/http';
import { PadSize } from '../data-transfer-objects/pad-size';
import { Pad } from '@/api/admin/data-transfer-objects/pad';
import { Rating } from '../../product-catalog/data-transfer-objects/rating';
import { Image } from '../../shared/data-transfer-objects/image';
import { PadOption } from '../data-transfer-objects/pad-option';
import { PadFilterLegend } from '../../product-catalog/data-transfer-objects/pad-filter-legend';
import { PagedArray } from '@/api/shared/data-transfer-objects/paged-array';
import { PadSeriesCreateRequest } from '../data-transfer-objects/requests/pad-series-create-request';
import { PadSeriesUpdateRequest } from '../data-transfer-objects/requests/pad-series-update-request';
import { Measurement, PagingOptions } from '@/api/shared';

export class PadSeriesService {
    async get(paging: PagingOptions = { pageNumber: 0, pageSize: 20 }): Promise<PagedArray<PadSeries>> {
        const res = await http.get('admin/product-catalog/pad-series', { params: paging });

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

    async getById(id: string): Promise<PadSeries | null> {
        const res = await http.get(`admin/product-catalog/pad-series/${id}`);

        if (res.data != null) {
            return this._map(res.data);
        } else {
            return null;
        }
    }

    async create(create: PadSeriesCreateRequest) {
        const res = await http.post('admin/product-catalog/pad-series', create);
        const ps = this._map(res.data);
        return ps;
    }

    async update(update: PadSeriesUpdateRequest) {
        const res = await http.put(`admin/product-catalog/pad-series/${update.id}`, update);
        const ps = this._map(res.data);
        return ps;
    }

    async delete(id: string) {
        await http.delete(`admin/product-catalog/pad-series/${id}`);
    }

    _map(ps: any): PadSeries {
        const series = new PadSeries(
            ps.id,
            ps.name,
            new Brand(ps.brand.id, ps.brand.name),
            ps.polisherTypes,
            (ps.sizes ?? []).map(
                (s: any) =>
                    new PadSize(
                        s.id,
                        { amount: s.diameter.amount, unit: s.diameter.unit },
                        s.thickness != null
                            ? { amount: s.thickness.amount, unit: s.thickness.unit }
                            : ((null as any) as Measurement)
                    )
            ),
            (ps.pads ?? []).map(
                (c: any) =>
                    new Pad(
                        c.id,
                        c.name,
                        c.category,
                        c.material,
                        c.texture,
                        c.color,
                        c.hasCenterHole,
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

export const padSeriesService = new PadSeriesService();
