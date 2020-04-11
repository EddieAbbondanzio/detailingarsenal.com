import { http } from '@/core';
import { Business } from '@/modules/app/api/settings/entities/business';
import { UpdateBusiness } from '@/modules/app/api/settings/data-transfer-objects/update-business';

export class BusinessService {
    public async getBusiness() {
        var res = await http.get('/settings/business');

        const b = new Business(res.data.name, res.data.address, res.data.phone);
        b.id = res.data.id;

        return b;
    }

    public async updateBusiness(updateBusiness: UpdateBusiness) {
        var res = await http.put(`/settings/business/${updateBusiness.id}`, updateBusiness);

        const b = new Business(res.data.name, res.data.address, res.data.phone);
        b.id = res.data.id;

        return b;
    }
}
