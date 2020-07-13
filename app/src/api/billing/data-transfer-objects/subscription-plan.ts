import { SubscriptionPlanPrice } from '@/api/billing/data-transfer-objects/subscription-plan-price';

/**
 * A plan that can be used to create subscriptions. A plan can hold multiple prices.
 * (EX one for monthly, one for yearly)
 */
export class SubscriptionPlan {
    constructor(
        public id: string,
        public name: string,
        public description: string | null,
        public prices: SubscriptionPlanPrice[]
    ) {}
}
