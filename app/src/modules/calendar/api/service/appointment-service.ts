import { Appointment } from '@/modules/calendar/api/entities/appointment';
import { CreateAppointment } from '@/modules/calendar/api/data-transfer-objects/create-appointment';
import { UpdateAppointment } from '@/modules/calendar/api/data-transfer-objects/update-appointment';
import { http } from '@/core';
import { AppointmentBlock } from '@/modules/calendar/api/entities/appointment-block';
import moment from 'moment';
import { CalendarRange } from '@/modules/app/store/calendar/calendar-range';

export class AppointmentService {
    public async get(day: Date, range: CalendarRange): Promise<Appointment[]> {
        const res = await http.get<any[]>('/appointment', {
            params: { range: 'day', date: moment(day).format('YYYY-MM-DD') }
        });

        if (res.data == null) {
            return [];
        }

        return res.data.map(d => this._map(d));
    }

    public async createAppointment(appointment: CreateAppointment): Promise<Appointment> {
        const res = await http.post<any>('/appointment', appointment);
        return this._map(res.data);
    }

    public async updateAppointment(appointment: UpdateAppointment): Promise<Appointment> {
        const res = await http.put<any>(`/appointment/${appointment.id}`, appointment);
        return this._map(res.data);
    }

    public async deleteAppointment(id: string): Promise<void> {
        await http.delete(`/appointment/${id}`);
    }

    _map(data: any): Appointment {
        const appointment = new Appointment(data.serviceId, data.clientId, data.price, [], data.notes);

        const blocks = data.blocks.map((t: any) => {
            const block = new AppointmentBlock(t.start, t.end);
            block.appointment = appointment;
            block.id = t.id;

            return block;
        });

        appointment.blocks = blocks;
        appointment.id = data.id;

        return appointment;
    }
}
