import { Entity } from '@/core';

export class SubscriptionPlanPrice extends Entity {
    constructor(public price: number, public interval: string) {
        super();
    }
}
