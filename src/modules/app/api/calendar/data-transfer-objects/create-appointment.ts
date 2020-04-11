import { CreateAppointmentBlock } from '@/modules/app/api/calendar/data-transfer-objects/create-appointment-block';
import { ClientInfo } from '@/modules/app/api/calendar/data-transfer-objects/client-info';

export type CreateAppointment = {
    serviceId: string;
    vehicleCategoryId: string;
    price: number;
    blocks: CreateAppointmentBlock[];
    clientId: string;
    notes?: string;
};
