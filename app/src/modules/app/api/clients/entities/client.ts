import { Entity } from '@/core';

export class Client extends Entity {
    public static NAME_MAX_LENGTH = 64;
    public static PHONE_NUMBER_MAX_LENGTH = 32;
    public static EMAIL_MAX_LENGTH = 256;

    constructor(public name: string, public phone: string | null = null, public email: string | null = null) {
        super();

        if (
            name.length > Client.NAME_MAX_LENGTH ||
            (phone != null && phone.length > Client.PHONE_NUMBER_MAX_LENGTH) ||
            (email != null && email.length > Client.EMAIL_MAX_LENGTH)
        ) {
            throw new RangeError();
        }
    }
}
