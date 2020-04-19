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

            return res.data.map((s: any) => {
                const service = new Service(
                    s.name,
                    s.description,
                    s.pricingMethod,
                    s.configurations.map((c: any) => new ServiceConfiguration(c.vehicleCategoryId, c.price, c.duration))
                );
                service.id = s.id;

                return service;
            });
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
        const s = new Service(
            res.data.name,
            res.data.description,
            res.data.pricingMethod,
            res.data.configurations.map((c: any) => new ServiceConfiguration(c.vehicleCategoryId, c.price, c.duration))
        );

        s.id = res.data.id;
        return s;
    }

    async updateService(updateService: UpdateService): Promise<Service> {
        const res = await http.put(`settings/service/${updateService.id}`, updateService);
        const s = new Service(
            res.data.name,
            res.data.description,
            res.data.pricingMethod,
            res.data.configurations.map((c: any) => new ServiceConfiguration(c.vehicleCategoryId, c.price, c.duration))
        );
        s.id = res.data.id;
        return s;
    }

    async deleteService(id: string) {
        await http.delete(`settings/service/${id}`);
    }
}
