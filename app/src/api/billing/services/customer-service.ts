import { http } from '@/api/core/http';
import { Subscription } from '@/api/billing/data-transfer-objects/subscription';
import { SubscriptionPlanPrice } from '@/api/billing/data-transfer-objects/subscription-plan-price';

export class CustomerService {
    async getCustomer() {
        throw new Error();
        // const res = await http.get('billing/subscription');

        // return new Subscription(
        //     res.data.name,
        //     new SubscriptionPlanPrice(res.data.price.amount, res.data.price.interval, res.data.price.billingId),
        //     res.data.status,
        //     res.data.nextPayment,
        //     res.data.trialStart,
        //     res.data.trialEnd
        // );
    }
}
