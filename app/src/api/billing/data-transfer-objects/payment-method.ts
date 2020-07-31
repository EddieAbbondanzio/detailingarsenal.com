import { ExpirationDate } from '@/api/billing/data-transfer-objects/expiration_date';

export class PaymentMethod {
    constructor(
        public brand: string,
        public last4: string,
        public isDefault: boolean,
        public expiration: ExpirationDate
    ) {}
}
