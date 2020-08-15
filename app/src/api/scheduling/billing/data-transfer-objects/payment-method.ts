import { ExpirationDate } from '@/api/scheduling/billing/data-transfer-objects/expiration-date';

export class PaymentMethod {
    constructor(
        public brand: string,
        public last4: string,
        public isDefault: boolean,
        public expiration: ExpirationDate
    ) {}
}
