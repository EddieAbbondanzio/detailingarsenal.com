import { SubscriptionPlanPrice } from '@/api/billing/data-transfer-objects/subscription-plan-price';
import { SubscriptionPlan } from '@/api/billing/data-transfer-objects/subscription-plan';
import { http } from '@/api/core/http';

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
                    : d.prices.map((p: any) => new SubscriptionPlanPrice(p.amount, p.interval, p.billingId));

            const plan = new SubscriptionPlan(d.id, d.name, d.description, prices);

            return plan;
        });
    }
}
