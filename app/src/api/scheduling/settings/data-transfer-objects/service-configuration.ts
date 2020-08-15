/**
 * Configuration that retains a price and time duration for a service.
 */
export class ServiceConfiguration {
    public static PRICE_MIN_VALUE = 0;
    public static DURATION_MIN_VALUE = 0;

    /**
     * Create a new service config.
     * @param vehicleCategoryId The ID of the vehicle category it applies to.
     * @param price The price in USD.
     * @param duration The amount of minutes it takes.
     */
    constructor(public id: string, public vehicleCategoryId: string, public price: number, public duration: number) {
        if (this.duration < ServiceConfiguration.DURATION_MIN_VALUE) {
            throw new RangeError('duration');
        }

        if (this.price < ServiceConfiguration.PRICE_MIN_VALUE) {
            throw new RangeError('price');
        }
    }
}
