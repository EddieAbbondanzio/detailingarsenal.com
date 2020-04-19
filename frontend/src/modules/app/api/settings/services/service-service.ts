import { Service } from '@/modules/app/api/settings/entities/service';
import { CreateService } from '@/modules/app/api/settings/data-transfer-objects/create-service';
import { UpdateService } from '@/modules/app/api/settings/data-transfer-objects/update-service';
import { http } from '@/core';
import { ServiceConfiguration } from '@/modules/app/api/settings/entities/service-configuration';

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

    async createService(createService: CreateService): Promise<Service> {
        const res = await http.post('settings/service', createService);
        const s = this._map(res.data);

        return s;
    }

    async updateService(updateService: UpdateService): Promise<Service> {
        const res = await http.put(`settings/service/${updateService.id}`, updateService);
        const s = this._map(res.data);
        return s;
    }

    async deleteService(id: string) {
        await http.delete(`settings/service/${id}`);
    }

    _map(d: any): Service {
        const s = new Service(
            d.name,
            d.description,
            d.pricingMethod,
            d.configurations.map((c: any) => {
                const config = new ServiceConfiguration(c.vehicleCategoryId, c.price, c.duration);
                config.id = c.id;

                return config;
            })
        );
        s.id = d.id;
        return s;
    }
}
