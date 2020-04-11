import { CreateAppointmentBlock } from '@/modules/app/api/calendar/data-transfer-objects/create-appointment-block';

export type UpdateAppointment = {
    id: string;
    serviceId: string;
    vehicleCategoryId: string;
    price: number;
    blocks: CreateAppointmentBlock[];
    client: string | { name: string; phone?: string; email?: string };
    notes: string;
};
