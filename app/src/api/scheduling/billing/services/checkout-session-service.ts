import { http } from '@/api/shared/http';

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

export const checkoutSessionService = new CheckoutSessionService();
