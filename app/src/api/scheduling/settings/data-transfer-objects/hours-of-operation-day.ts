export class HoursOfOperationDay {
    constructor(public day: number, public open: number, public close: number, public enabled: boolean = true) {}

    containsTime(time: number) {
        return time >= this.open && time < this.close;
    }
}
