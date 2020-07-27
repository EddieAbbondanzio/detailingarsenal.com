import { http } from '@/api/core/http';
import { Subscription } from '@/api/billing/data-transfer-objects/subscription';
import { SubscriptionPlanPrice } from '@/api/billing/data-transfer-objects/subscription-plan-price';
import { Customer } from '@/api/billing/data-transfer-objects/customer';
import { PaymentMethod } from '@/api/billing/data-transfer-objects/payment-method';

export class CustomerService {
    async getCustomer() {
        const res = await http.get('billing/customer');

        const c = new Customer(
            new Subscription(
                res.data.subscription.planName,
                new SubscriptionPlanPrice(
                    res.data.subscription.price.amount,
                    res.data.subscription.price.interval,
                    res.data.subscription.price.billingId
                ),
                res.data.subscription.status,
                res.data.subscription.nextPayment,
                res.data.subscription.trialStart,
                res.data.subscription.trialEnd,
                res.data.subscription.cancellingAtPeriodEnd
            )
        );

        if (res.data.paymentMethod != null) {
            c.paymentMethod = new PaymentMethod(res.data.paymentMethod.brand, res.data.paymentMethod.last4);
        }

        return c;
    }

    async cancelSubscription() {
        await http.delete('billing/customer/subscription');
    }

    async undoCancellingSubscription() {
        await http.patch('billing/customer/subscription');
    }
}
