import { Subscription } from '@/api/scheduling/billing/data-transfer-objects/subscription';
import { PaymentMethod } from '@/api/scheduling/billing/data-transfer-objects/payment-method';

export class Customer {
    constructor(public subscription: Subscription | null, public paymentMethods: PaymentMethod[] = []) {}
}
