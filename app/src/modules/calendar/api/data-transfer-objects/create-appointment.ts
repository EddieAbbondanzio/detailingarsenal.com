import { CreateAppointmentBlock } from '@/modules/calendar/api/data-transfer-objects/create-appointment-block';
import { ClientInfo } from '@/modules/calendar/api/data-transfer-objects/client-info';

export type CreateAppointment = {
    serviceId: string;
    price: number;
    blocks: CreateAppointmentBlock[];
    clientId: string;
    notes?: string;
};
