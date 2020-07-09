import { http } from '@/api/core/http';
import { loadStripe, Stripe } from '@stripe/stripe-js';

/**
 *
 */
export class CheckoutSessionService {
    stripe: Stripe = null!;

    async init() {
        const stripe = await loadStripe(
            'pk_test_51Gq6bjGGaXZMuOr2CVJGA5suAjPXCNz0ZyAPV2sZ6hymyOmmMMfqfEmSErHZlmzdiIW3ndaMK3oA5vPYXjqg8C4700k7gQraDC'
        );

        if (stripe == null) {
            throw new Error('Stripe was null');
        }

        this.stripe = stripe;
    }

    async createSession(priceBillingId: string) {
        const res = await http.post('billing/checkout-session', {
            priceBillingId
        });

        return res.data.checkoutSessionId;
    }

    async enterSession(sessionId: string) {
        await this.stripe.redirectToCheckout({
            sessionId
        });
    }
}
