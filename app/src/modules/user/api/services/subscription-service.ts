import { http } from '@/core';

export class SubscriptionService {
    async getUserSubscription() {
        const res = await http.get('billing/subscription');
        console.log(res);
    }
}
