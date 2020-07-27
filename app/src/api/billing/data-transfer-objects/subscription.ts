import { SubscriptionPlanPrice } from '@/api/billing/data-transfer-objects/subscription-plan-price';
import { BillingInterval } from '@/api/billing/data-transfer-objects/billing-interval';
import { SubscriptionStatus } from '@/api/billing/data-transfer-objects/subscription-status';
import moment from 'moment';

/**
 * Subscription for a user. Associates a plan, and a specific price along with the current status of the plan.
 */
export class Subscription {
    constructor(
        public planName: string,
        public price: SubscriptionPlanPrice,
        public status: SubscriptionStatus,
        public nextPayment: Date,
        public trialStart: Date,
        public trialEnd: Date,
        public cancellingAtPeriodEnd: boolean
    ) {}

    get trialDaysRemaining() {
        const today = moment();
        const trialEnd = moment(this.trialEnd);

        return trialEnd.diff(today, 'days');
    }
}
