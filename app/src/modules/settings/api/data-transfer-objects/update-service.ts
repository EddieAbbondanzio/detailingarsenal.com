import { ServiceConfiguration } from '@/modules/settings/api/entities/service-configuration';
import { VehicleCategory } from '@/modules/app/api';
import { ServicePricingMethod } from '@/modules/settings/api/value-objects/service-pricing-method';

export type UpdateService = {
    id: string;
    name: string;
    description?: string;
    pricingMethod: ServicePricingMethod;
    configurations: { vehicleCategoryId: string | null; price: number; duration: number }[];
};
