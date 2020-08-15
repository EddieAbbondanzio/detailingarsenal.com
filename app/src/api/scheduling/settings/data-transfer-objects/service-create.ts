import { ServicePricingMethod } from '@/api/scheduling/settings/data-transfer-objects/service-pricing-method';

export type ServiceCreate = {
    name: string;
    description?: string;
    pricingMethod: ServicePricingMethod;
    configurations: { vehicleCategoryId: string | null; price: number; duration: number }[];
};
