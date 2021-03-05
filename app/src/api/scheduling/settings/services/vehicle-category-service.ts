import { http } from '@/api/shared/http';
import { VehicleCategory } from '@/api/scheduling/settings/data-transfer-objects/vehicle-category';
import { VehicleCategoryCreate } from '@/api/scheduling/settings/data-transfer-objects/vehicle-category-create';
import { VehicleCategoryUpdate } from '@/api/scheduling/settings/data-transfer-objects/vehicle-category-update';

export class VehicleCategoryService {
    public async getVehicleCategories() {
        try {
            var res = await http.get<[{ id: string; name: string; description?: string }]>(
                '/settings/vehicle-category'
            );

            return res.data.map(vc => {
                var newOne = new VehicleCategory(vc.id, vc.name, vc.description);
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

    public async createVehicleCategory(create: VehicleCategoryCreate) {
        var res = await http.post('/settings/vehicle-category', create);

        var vc = new VehicleCategory(res.data.name, res.data.description);
        vc.id = res.data.id;

        return vc;
    }

    public async updateVehicleCategory(update: VehicleCategoryUpdate) {
        var res = await http.put(`/settings/vehicle-category/${update.id}`, update);

        var vc = new VehicleCategory(res.data.name, res.data.description);
        vc.id = res.data.id;

        return vc;
    }

    public async deleteVehicleCategory(vehicleCategory: VehicleCategory) {
        await http.delete(`/settings/vehicle-category/${vehicleCategory.id}`);
    }
}

export const vehicleCategoryService = new VehicleCategoryService();
