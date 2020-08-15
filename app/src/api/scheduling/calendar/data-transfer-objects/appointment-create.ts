import { AppointmentBlockCreate } from '@/api/scheduling/calendar/data-transfer-objects/appointment-block-create';

export type AppointmentCreate = {
    serviceId: string;
    price: number;
    blocks: AppointmentBlockCreate[];
    clientId: string;
    notes?: string;
};
