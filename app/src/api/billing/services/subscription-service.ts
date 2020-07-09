import { http } from '@/api/core/http';
import { Subscription } from '@/api/billing/data-transfer-objects/subscription';
import { SubscriptionPlanInfo } from '@/api/billing/data-transfer-objects/subscription-plan-info';
import { SubscriptionPlanPrice } from '@/api/billing/data-transfer-objects/subscription-plan-price';

export class SubscriptionService {
    async getUserSubscription() {
        const res = await http.get('billing/subscription');

        return new Subscription(
            new SubscriptionPlanInfo(res.data.id, res.data.name, res.data.description),
            new SubscriptionPlanPrice(res.data.price.amount, res.data.price.interval, res.data.price.billingId),
            res.data.status
        );
    }
}
