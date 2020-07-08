import { Entity } from '@/core';
import { ValueObject } from '@/core/api/value-object';

export class SubscriptionPlanPrice extends ValueObject {
    constructor(public price: number = 0, public interval: string) {
        super();
    }
}
