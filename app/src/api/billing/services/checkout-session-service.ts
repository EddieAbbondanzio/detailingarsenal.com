import { http } from '@/api/core/http';

/**
 *
 */
export class CheckoutSessionService {
    async createSession(priceBillingId: string) {
        const res = await http.post('billing/checkout-session', {
            priceBillingId
        });

        return res.data.checkoutSessionId as string;
    }
}
