import { ServicePricingMethod } from '@/modules/settings/api/value-objects/service-pricing-method';

export type CreateService = {
    name: string;
    description?: string;
    pricingMethod: ServicePricingMethod;
    configurations: { vehicleCategoryId: string | null; price: number; duration: number }[];
};
