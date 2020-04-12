import { Entity } from '@/core';

export class HoursOfOperationDay extends Entity {
    constructor(public day: number, public open: number, public close: number, public enabled: boolean = true) {
        super();
    }

    containsTime(time: number) {
        return time >= this.open && time < this.close;
    }
}
