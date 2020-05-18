import { ServiceConfiguration } from '@/modules/settings/api/entities/service-configuration';
import { VehicleCategory } from '@/modules/settings/api/entities/vehicle-category';
import { Entity } from '@/core';
import { ServicePricingMethod } from '@/modules/settings/api/value-objects/service-pricing-method';

/**
 * Service archtype that can be used to create jobs from.
 */
export class Service extends Entity {
    public static NAME_MAX_LENGTH = 32;
    public static DESCRIPTION_MAX_LENGTH = 512;

    /**
     * Create a new service.
     * @param name The title of the service.
     * @param description Text description of the service.
     * @param configurations The configurations for pricing and duration.
     */
    constructor(
        public name: string,
        public description: string | null,
        public pricingMethod: ServicePricingMethod,
        public configurations: ServiceConfiguration[] = []
    ) {
        super();

        if (name.length > Service.NAME_MAX_LENGTH) {
            throw new RangeError('name');
        }

        if (description != null && description.length > Service.DESCRIPTION_MAX_LENGTH) {
            throw new RangeError('description');
        }
    }

    getConfigurationForVehicleCategory(vehicleCategory: VehicleCategory) {
        return this.configurations.find(vc => vc.vehicleCategoryId == vehicleCategory.id);
    }
}
