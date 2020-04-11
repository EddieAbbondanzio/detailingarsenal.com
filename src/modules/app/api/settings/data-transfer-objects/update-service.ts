import { ServiceConfiguration } from '@/modules/app/api/settings/entities/service-configuration';
import { VehicleCategory } from '@/modules/app/api';

export type UpdateService = {
    id: string;
    name: string;
    description?: string;
    configurations: { vehicleCategoryId: string | null; price: number; duration: number }[];
};
