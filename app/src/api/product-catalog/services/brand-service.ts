import { Brand } from '@/api';
import { http } from '@/api/core/http';
import { BrandCreateRequest } from '../data-transfer-objects/requests/brand-create-request';
import { BrandUpdateRequest } from '../data-transfer-objects/requests/brand-update-request';

export class BrandService {
    async get() {
        try {
            const res = await http.get('product-catalog/brands');

            if (res.data == null) {
                return [];
            }

            return res.data.map((s: any) => this._map(s));
        } catch (e) {
            if (e.response == null || e.response.status == 404) {
                return [];
            } else {
                throw e;
            }
        }
    }

    async create(create: BrandCreateRequest) {
        const res = await http.post('product-catalog/brands', create);
        const b = this._map(res.data);
        return b;
    }

    async update(update: BrandUpdateRequest) {
        const res = await http.put(`product-catalog/brands/${update.id}`, update);
        const b = this._map(res.data);
        return b;
    }

    async delete(id: string) {
        await http.delete(`product-catalog/brands/${id}`);
    }

    _map(b: any): Brand {
        return new Brand(b.id, b.name);
    }
}
