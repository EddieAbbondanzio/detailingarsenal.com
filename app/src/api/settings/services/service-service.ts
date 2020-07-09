import { Service } from '@/api/settings/data-transfer-objects/service';
import { http } from '@/api/core/http';
import { ServiceCreate } from '@/api/settings/data-transfer-objects/service-create';
import { ServiceUpdate } from '@/api/settings/data-transfer-objects/service-update';
import { ServiceConfiguration } from '@/api/settings/data-transfer-objects/service-configuration';

// Don't think about the name to hard.
export class ServiceService {
    async getServices(): Promise<Service[]> {
        try {
            const res = await http.get('settings/service');

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

    async createService(createService: ServiceCreate): Promise<Service> {
        const res = await http.post('settings/service', createService);
        const s = this._map(res.data);

        return s;
    }

    async updateService(updateService: ServiceUpdate): Promise<Service> {
        const res = await http.put(`settings/service/${updateService.id}`, updateService);
        const s = this._map(res.data);
        return s;
    }

    async deleteService(id: string) {
        await http.delete(`settings/service/${id}`);
    }

    _map(d: any): Service {
        const s = new Service(
            d.id,
            d.name,
            d.description,
            d.pricingMethod,
            d.configurations.map((c: any) => {
                const config = new ServiceConfiguration(c.id, c.vehicleCategoryId, c.price, c.duration);
                config.id = c.id;

                return config;
            })
        );
        return s;
    }
}
