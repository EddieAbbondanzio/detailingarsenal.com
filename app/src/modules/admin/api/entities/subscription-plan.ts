import { SubscriptionPlanPrice } from '@/modules/admin/api/entities/subscription-plan-price';
import { Entity } from '@/core';

export class SubscriptionPlan extends Entity {
    constructor(public name: string, public prices: SubscriptionPlanPrice[], public roleId: string | null = null) {
        super();
    }
}
