import { HoursOfOperation } from '@/modules/settings/api/entities/hours-of-operation';
import { HoursOfOperationDay } from '@/modules/settings/api/entities/hours-of-operation-day';
import { http } from '@/core';
import { UpdateHoursOfOperation } from '@/modules/settings/api/data-transfer-objects/update-hours-of-operation';

export class HoursOfOperationService {
    async getHoursOfOperation(): Promise<HoursOfOperation> {
        var res = await http.get('/settings/hours-of-operation');

        var days =
            res.data.days != null
                ? res.data.days.map((d: any) => new HoursOfOperationDay(d.day, d.open, d.close, d.enabled))
                : [];
        var hours = new HoursOfOperation(days);
        hours.id = res.data.id;

        return hours;
    }

    async updateHoursOfOperation(update: UpdateHoursOfOperation): Promise<HoursOfOperation> {
        var res = await http.put(`/settings/hours-of-operation/${update.id}`, update);

        var days =
            res.data.days != null
                ? res.data.days.map((d: any) => new HoursOfOperationDay(d.day, d.open, d.close, d.enabled))
                : [];
        var h = new HoursOfOperation(days);
        h.id = res.data.id;

        return h;
    }
}
