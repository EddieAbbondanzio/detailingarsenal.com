import { Appointment } from '@/api/scheduling/calendar/data-transfer-objects/appointment';
import { AppointmentCreate } from '@/api/scheduling/calendar/data-transfer-objects/appointment-create';
import { AppointmentUpdate } from '@/api/scheduling/calendar/data-transfer-objects/appointment-update';
import { AppointmentBlock } from '@/api/scheduling/calendar/data-transfer-objects/appointment-block';
import moment from 'moment';
import { CalendarRange } from '@/modules/scheduling/calendar/store/calendar-range';
import { http } from '@/api/shared/http';

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

    public async createAppointment(appointment: AppointmentCreate): Promise<Appointment> {
        const res = await http.post<any>('/appointment', appointment);
        return this._map(res.data);
    }

    public async updateAppointment(appointment: AppointmentUpdate): Promise<Appointment> {
        const res = await http.put<any>(`/appointment/${appointment.id}`, appointment);
        return this._map(res.data);
    }

    public async deleteAppointment(id: string): Promise<void> {
        await http.delete(`/appointment/${id}`);
    }

    _map(data: any): Appointment {
        const appointment = new Appointment(data.id, data.serviceId, data.clientId, data.price, [], data.notes);

        const blocks = data.blocks.map((t: any) => {
            const block = new AppointmentBlock(t.id, t.start, t.end);
            block.appointment = appointment;
            block.id = t.id;

            return block;
        });

        appointment.blocks = blocks;
        appointment.id = data.id;

        return appointment;
    }
}

export const appointmentService = new AppointmentService();
