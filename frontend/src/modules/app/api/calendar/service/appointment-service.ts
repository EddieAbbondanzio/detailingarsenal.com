import { Appointment } from '@/modules/app/api/calendar/entities/appointment';
import { CreateAppointment } from '@/modules/app/api/calendar/data-transfer-objects/create-appointment';
import { UpdateAppointment } from '@/modules/app/api/calendar/data-transfer-objects/update-appointment';
import { http } from '@/core';
import { AppointmentBlock } from '@/modules/app/api/calendar/entities/appointment-block';
import moment from 'moment';

export class AppointmentService {
    public async getAppointmentsForWeek(day: Date): Promise<Appointment[]> {
        const res = await http.get<any[]>('/appointment', {
            params: { range: 'week', date: moment(day).format('YYYY-MM-DD') }
        });
        return res.data.map(d => this._map(d));
    }

    public async createAppointment(appointment: CreateAppointment): Promise<Appointment> {
        const res = await http.post<any>('/appointment', appointment);
        return this._map(res.data);
    }

    public async updateAppointment(appointment: UpdateAppointment): Promise<Appointment> {
        const res = await http.put<any>('/appointment', appointment);
        return this._map(res.data);
    }

    public async deleteAppointment(id: string): Promise<void> {
        await http.delete(`/appointment/${id}`);
    }

    _map(data: any): Appointment {
        const appointment = new Appointment(
            data.serviceId,
            data.vehicleCategoryId,
            data.clientId,
            data.price,
            [],
            data.notes
        );

        const blocks = data.blocks.map((t: any) => {
            const block = new AppointmentBlock(t.date, t.time, t.duration);
            block.appointment = appointment;
            block.id = t.id;

            return block;
        });

        appointment.blocks = blocks;
        appointment.id = data.id;

        return appointment;
    }
}
