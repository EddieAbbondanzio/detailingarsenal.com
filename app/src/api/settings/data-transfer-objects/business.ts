/**
 * Business information such as the name, address, and phone number.
 */
export class Business {
    public static NAME_MAX_LENGTH = 64;
    public static ADDRESS_MAX_LENGTH = 128;
    public static PHONE_MAX_LENGTH = 32;

    /**
     * Create a new business entity.
     * @param name The official name of the business.
     * @param address The street address.
     * @param phone The contact phone number.
     */
    constructor(public id: string, public name?: string, public address?: string, public phone?: string) {
        if (name != null && name.length > Business.NAME_MAX_LENGTH) {
            throw new RangeError('name');
        }

        if (address != null && address.length > Business.ADDRESS_MAX_LENGTH) {
            throw new RangeError('address');
        }

        if (phone != null && phone.length > Business.PHONE_MAX_LENGTH) {
            throw new RangeError('phone');
        }
    }
}
