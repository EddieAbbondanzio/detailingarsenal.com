import { ServiceConfiguration } from '@/modules/app/api/settings/entities/service-configuration';
import { VehicleCategory } from '@/modules/app/api';
import { ServicePricingMethod } from '@/modules/app/api/settings/value-objects/service-pricing-method';

export type CreateService = {
    name: string;
    description?: string;
    pricingMethod: ServicePricingMethod;
    configurations: { vehicleCategoryId: string | null; price: number; duration: number }[];
};
