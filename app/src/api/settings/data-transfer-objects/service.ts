import { ServiceConfiguration } from '@/api/settings/data-transfer-objects/service-configuration';
import { ServicePricingMethod } from '@/api/settings/data-transfer-objects/service-pricing-method';
import { VehicleCategory } from '@/api/settings/data-transfer-objects/vehicle-category';

/**
 * Service archtype that can be used to create jobs from.
 */
export class Service {
    public static NAME_MAX_LENGTH = 32;
    public static DESCRIPTION_MAX_LENGTH = 512;

    /**
     * Create a new service.
     * @param name The title of the service.
     * @param description Text description of the service.
     * @param configurations The configurations for pricing and duration.
     */
    constructor(
        public id: string,
        public name: string,
        public description: string | null,
        public pricingMethod: ServicePricingMethod,
        public configurations: ServiceConfiguration[] = []
    ) {
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
