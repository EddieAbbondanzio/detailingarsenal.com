import { AppointmentBlockCreate } from '@/api/calendar/data-transfer-objects/appointment-block-create';

export type AppointmentUpdate = {
    id: string;
    serviceId: string;
    price: number;
    blocks: AppointmentBlockCreate[];
    clientId: string;
    notes?: string;
};
