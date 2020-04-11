import { CreateVehicleCategory } from '@/modules/app/api/settings/data-transfer-objects/create-vehicle-category';
import { UpdateVehicleCategory } from '@/modules/app/api/settings/data-transfer-objects/update-vehicle-category';
import { http } from '@/core';
import { VehicleCategory } from '@/modules/app/api/settings/entities/vehicle-category';

export class VehicleCategoryService {
    public async getVehicleCategories() {
        try {
            var res = await http.get<[{ id: string; name: string; description?: string }]>(
                '/settings/vehicle-category'
            );

            return res.data.map(vc => {
                var newOne = new VehicleCategory(vc.name, vc.description);
                newOne.id = vc.id;

                return newOne;
            });
        } catch (e) {
            if (e.response.status == 404) {
                return [];
            } else {
                throw e;
            }
        }
    }

    public async createVehicleCategory(create: CreateVehicleCategory) {
        var res = await http.post('/settings/vehicle-category', create);

        var vc = new VehicleCategory(res.data.name, res.data.description);
        vc.id = res.data.id;

        return vc;
    }

    public async updateVehicleCategory(update: UpdateVehicleCategory) {
        var res = await http.put(`/settings/vehicle-category/${update.id}`, update);

        var vc = new VehicleCategory(res.data.name, res.data.description);
        vc.id = res.data.id;

        return vc;
    }

    public async deleteVehicleCategory(vehicleCategory: VehicleCategory) {
        await http.delete(`/settings/vehicle-category/${vehicleCategory.id}`);
    }
}
