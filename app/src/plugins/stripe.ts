import { loadStripe, Stripe } from '@stripe/stripe-js';

export let stripe: Stripe = null!;

export async function loadStripeJs() {
    stripe = (await loadStripe(process.env.VUE_APP_STRIPE_KEY!))!;
}
