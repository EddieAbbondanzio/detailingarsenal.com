import { http } from '@/core';
import { Subscription, SubscriptionPrice } from '@/modules/user/api/entities/subscription';

export class SubscriptionService {
    async getUserSubscription() {
        const res = await http.get('billing/subscription');

        return new Subscription(
            res.data.name,
            res.data.status,
            new SubscriptionPrice(res.data.price.amount, res.data.price.interval, res.data.price.billingId)
        );
    }
}
