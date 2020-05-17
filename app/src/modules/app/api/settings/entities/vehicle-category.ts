import { Entity } from '@/core';

/**
 * Category to group price and time durations for services.
 */
export class VehicleCategory extends Entity {
    public static NAME_MAX_LENGTH = 32;
    public static DESCRIPTION_MAX_LENGTH = 128;

    /**
     * Create a new vehicle category.
     * @param name The display name of the category.
     * @param description The text description.
     */
    constructor(public name: string, public description: string = '') {
        super();

        if (name.length > VehicleCategory.NAME_MAX_LENGTH) {
            throw new RangeError('name');
        }

        if (description.length > VehicleCategory.DESCRIPTION_MAX_LENGTH) {
            throw new RangeError('description');
        }
    }
}
