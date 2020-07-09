import { HoursOfOperation } from '@/api/settings/data-transfer-objects/hours-of-operation';
import { HoursOfOperationDay } from '@/api/settings/data-transfer-objects/hours-of-operation-day';
import { http } from '@/api/core/http';
import { HoursOfOperationUpdate } from '@/api/settings/data-transfer-objects/hours-of-operation-update';

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

    async updateHoursOfOperation(update: HoursOfOperationUpdate): Promise<HoursOfOperation> {
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
