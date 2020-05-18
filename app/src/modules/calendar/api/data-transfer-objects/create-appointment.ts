import { CreateAppointmentBlock } from '@/modules/calendar/api/data-transfer-objects/create-appointment-block';

export type CreateAppointment = {
    serviceId: string;
    price: number;
    blocks: CreateAppointmentBlock[];
    clientId: string;
    notes?: string;
};
