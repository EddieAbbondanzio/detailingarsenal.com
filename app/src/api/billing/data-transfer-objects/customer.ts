import { Subscription } from '@/api/billing/data-transfer-objects/subscription';
import { PaymentMethod } from '@/api/billing/data-transfer-objects/payment-method';

export class Customer {
    constructor(public subscription: Subscription | null, public paymentMethod: PaymentMethod | null = null) {}
}
