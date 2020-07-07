import { http } from '@/core';
import { SubscriptionPlanPrice } from '@/modules/admin/api/entities/subscription-plan-price';
import { SubscriptionPlan } from '@/modules/admin/api/entities/subscription-plan';
import { UpdateSubscriptionPlan } from '@/modules/admin/api/data-transfer-objects/update-subscription-plan';

export class SubscriptionPlanService {
    async getPlans() {
        const res = await http.get('/billing/subscription-plan');
        return this._map(res.data);
    }

    async getDefault() {
        const res = await http.get('/billing/subscription-plan/default');
        return this._map([res.data])[0];
    }

    async refreshPlans() {
        const res = await http.post('/billing/subscription-plan/refresh');
        return this._map(res.data);
    }

    _map(data: any[]) {
        return data.map(d => {
            const prices =
                d.prices == null
                    ? []
                    : d.prices.map((p: any) => {
                          const price = new SubscriptionPlanPrice(p.price, p.interval);
                          price.id = p.id;

                          return price;
                      });

            const plan = new SubscriptionPlan(d.name, prices);
            plan.id = d.id;

            return plan;
        });
    }
}
