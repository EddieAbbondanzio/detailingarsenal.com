import { SubscriptionPlanPrice } from '@/api/billing/data-transfer-objects/subscription-plan-price';
import { SubscriptionPlanInfo } from '@/api/billing/data-transfer-objects/subscription-plan-info';

/**
 * A plan that can be used to create subscriptions. A plan can hold multiple prices.
 * (EX one for monthly, one for yearly)
 */
export class SubscriptionPlan {
    constructor(public info: SubscriptionPlanInfo, public prices: SubscriptionPlanPrice[]) {}
}
