import { SubscriptionPlanPrice } from '@/modules/admin/api/entities/subscription-plan-price';

export class Subscription {
    constructor(public name: string, public status: string, public price: SubscriptionPrice) {}
}

export class SubscriptionPrice {
    constructor(public amount: number, public interval: SubscriptionPriceInterval, public billingId: string) {}
}

export type SubscriptionPriceInterval = 'year' | 'month';
