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

    _map(data: any[]) {
        return data.map(d => {
            const prices =
                d.prices == null ? [] : d.prices.map((p: any) => new SubscriptionPlanPrice(p.price, p.interval));
            return new SubscriptionPlan(d.name, prices);
        });
    }
}
