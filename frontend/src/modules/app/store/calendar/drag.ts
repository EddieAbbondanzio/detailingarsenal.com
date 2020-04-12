import { DragPoint } from '@/modules/app/store/calendar/drag-point';
import { Selection } from '@/modules/app/store/calendar/selection';
import moment from 'moment';

export class Drag {
    constructor(public start: DragPoint | null = null, public end: DragPoint | null = null) {}

    clear() {
        this.start = null;
        this.end = null;
    }

    toSelections(): Selection[] {
        if (this.start == null || this.end == null) {
            return [];
        }

        // Determine start day, and end
        const moments = [moment(this.start.day), moment(this.end.day)];
        const startM = moment.min(moments);
        const endM = moment.max(moments);

        // Determine start time and end
        const startT = Math.min(this.start.time, this.end.time);
        const endT = Math.max(this.start.time, this.end.time);

        // Calculate day / time differences
        const daysBetween = endM.diff(startM, 'days') + 1;
        const timeBetween = endT - startT;

        // Generate the selections
        const selections = [];
        const duration = Math.max(endT - startT, 15);

        for (let i = 0; i < daysBetween; i++) {
            // Determine the next day. Clone startM to prevent from manipulating it.
            const d = moment(startM).add(i, 'days');
            selections.push(new Selection(d.toDate(), startT, duration));
        }

        return selections;
    }
}
