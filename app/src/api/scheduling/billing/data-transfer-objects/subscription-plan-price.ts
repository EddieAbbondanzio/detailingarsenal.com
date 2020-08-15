import { BillingInterval } from '@/api/scheduling/billing/data-transfer-objects/billing-interval';

/**
 * A price for a subscription plan.
 */
export class SubscriptionPlanPrice {
    constructor(public amount: number = 0, public interval: BillingInterval, public billingId: string) {}
}
