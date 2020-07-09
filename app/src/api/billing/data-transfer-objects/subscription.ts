import { SubscriptionPlanPrice } from '@/api/billing/data-transfer-objects/subscription-plan-price';
import { BillingInterval } from '@/api/billing/data-transfer-objects/billing-interval';
import { SubscriptionStatus } from '@/api/billing/data-transfer-objects/subscription-status';
import { SubscriptionPlanInfo } from '@/api/billing/data-transfer-objects/subscription-plan-info';

/**
 * Subscription for a user. Associates a plan, and a specific price along with the current status of the plan.
 */
export class Subscription {
    constructor(
        public plan: SubscriptionPlanInfo,
        public price: SubscriptionPlanPrice,
        public status: SubscriptionStatus
    ) {}
}
