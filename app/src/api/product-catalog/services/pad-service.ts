import { http, PagedArray, PagingOptions } from '@/api/shared';
import { Pad } from '@/api/product-catalog';

export class PadService {
    async getAll(req: { paging: PagingOptions } | null = null): Promise<PagedArray<Pad>> {
        const res = await http.get('product-catalog/pads');

        const vals = res.data.values.map((d: any) => this.map(d));
        return {
            paging: res.data.paging,
            values: vals
        };
    }

    async get(id: string): Promise<Pad | null> {
        const res = await http.get(`product-catalog/pads/${id}`);

        if (res.data != null) {
            return this.map(res.data);
        } else {
            return null;
        }
    }

    map(raw: any): Pad {
        return new Pad(
            raw.id,
            raw.name,
            raw.series,
            raw.brand,
            raw.category,
            raw.color,
            raw.material,
            raw.texture,
            raw.cut,
            raw.finish,
            raw.rating,
            raw.polisherTypes,
            raw.hasCenterHole,
            raw.image
        );
    }
}

export const padService = new PadService();
