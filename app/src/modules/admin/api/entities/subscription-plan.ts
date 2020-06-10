import { SubscriptionPlanPrice } from '@/modules/admin/api/entities/subscription-plan-price';

export class SubscriptionPlan {
    constructor(public name: string, public prices: SubscriptionPlanPrice[]) {}
}
