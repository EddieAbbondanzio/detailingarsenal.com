import { http } from '@/api/shared/http';
import { Subscription } from '@/api/scheduling/billing/data-transfer-objects/subscription';
import { SubscriptionPlanPrice } from '@/api/scheduling/billing/data-transfer-objects/subscription-plan-price';
import { Customer } from '@/api/scheduling/billing/data-transfer-objects/customer';
import { PaymentMethod } from '@/api/scheduling/billing/data-transfer-objects/payment-method';
import { ExpirationDate } from '@/api/scheduling/billing/data-transfer-objects/expiration-date';
import { Period } from '@/api/scheduling/billing/data-transfer-objects/period';

export class CustomerService {
    async get() {
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
                new Period(res.data.subscription.trialPeriod.start, res.data.subscription.trialPeriod.end),
                new Period(res.data.subscription.period.start, res.data.subscription.period.end),
                res.data.subscription.cancellingAtPeriodEnd
            )
        );

        if (Array.isArray(res.data.paymentMethods)) {
            for (let i = 0; i < res.data.paymentMethods.length; i++) {
                const raw = res.data.paymentMethods[i];

                c.paymentMethods.push(
                    new PaymentMethod(
                        raw.brand,
                        raw.last4,
                        raw.isDefault,
                        new ExpirationDate(raw.expirationMonth, raw.expirationYear)
                    )
                );
            }
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

export const customerService = new CustomerService();
