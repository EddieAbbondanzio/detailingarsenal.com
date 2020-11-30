import { SubscriptionPlanPrice } from '@/api/scheduling/billing/data-transfer-objects/subscription-plan-price';

/**
 * A plan that can be used to create subscriptions. A plan can hold multiple prices.
 * (EX one for monthly, one for yearly)
 */
export class SubscriptionPlan {
    constructor(
        public id: string,
        public name: string,
        public description: string | null,
        public roleId: string | null,
        public prices: SubscriptionPlanPrice[]
    ) { }

    get yearlyPrice() {
        const p = this.prices.find(p => p.interval == 'year');

        if (p == null) {
            throw new Error('No yearly price specified');
        }

        return p;
    }

    get monthlyPrice() {
        const p = this.prices.find(p => p.interval == 'month');

        if (p == null) {
            throw new Error('No monthly price specified');
        }

        return p;
    }
}
