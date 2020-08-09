import { SubscriptionPlanPrice } from '@/api/billing/data-transfer-objects/subscription-plan-price';
import { BillingInterval } from '@/api/billing/data-transfer-objects/billing-interval';
import { SubscriptionStatus } from '@/api/billing/data-transfer-objects/subscription-status';
import moment from 'moment';
import { Period } from '@/api/billing/data-transfer-objects/period';

/**
 * Subscription for a user. Associates a plan, and a specific price along with the current status of the plan.
 */
export class Subscription {
    constructor(
        public planName: string,
        public price: SubscriptionPlanPrice,
        public status: SubscriptionStatus,
        public trialPeriod: Period,
        public period: Period,
        public cancellingAtPeriodEnd: boolean
    ) {}

    get trialDaysRemaining() {
        const today = moment();
        const trialEnd = moment(this.trialPeriod.end);

        return trialEnd.diff(today, 'days');
    }
}
