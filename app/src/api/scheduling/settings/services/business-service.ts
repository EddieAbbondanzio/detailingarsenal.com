import { http } from '@/api/core/http';
import { BusinessUpdate } from '@/api/scheduling/settings/data-transfer-objects/business-update';
import { Business } from '@/api/scheduling/settings/data-transfer-objects/business';

export class BusinessService {
    public async getBusiness() {
        var res = await http.get('/settings/business');

        const b = new Business(res.data.id, res.data.name, res.data.address, res.data.phone);

        return b;
    }

    public async updateBusiness(updateBusiness: BusinessUpdate) {
        var res = await http.put(`/settings/business/${updateBusiness.id}`, updateBusiness);

        const b = new Business(res.data.id, res.data.name, res.data.address, res.data.phone);

        return b;
    }
}
