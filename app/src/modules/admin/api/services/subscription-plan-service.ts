import { http } from '@/core';
import { SubscriptionPlanPrice } from '@/modules/admin/api/entities/subscription-plan-price';
import { SubscriptionPlan } from '@/modules/admin/api/entities/subscription-plan';

export class SubscriptionPlanService {
    async getPlans() {
        const res = await http.get('/billing/subscription-plan');
        return this._map(res.data);
    }

    async refreshPlans() {
        const res = await http.post('/billing/subscription-plan/refresh');
        return this._map(res.data);
    }

    _map(data: any) {
        if (data == null) {
            return null;
        }

        const prices =
            data.prices == null ? [] : (data.prices as any[]).map(p => new SubscriptionPlanPrice(p.price, p.interval));
        return new SubscriptionPlan(data.name, prices);
    }
}
