import { Module, VuexModule, Mutation, Action } from 'vuex-module-decorators';
import { InitableModule } from '@/core/store/initable-module';
import { CalendarView } from '@/modules/app/store/calendar/calendar-view';
import moment from 'moment';
import { TimeUtils } from '@/core';
import { Appointment } from '@/modules/app/api/calendar/entities/appointment';
import { AppointmentBlock } from '@/modules/app/api/calendar/entities/appointment-block';

/**
 * Store for the Calendar view.
 */
@Module({ namespaced: true, name: 'calendar' })
export default class CalendarStore extends InitableModule {
    view: CalendarView = 'day';
    date: Date = new Date();

    blocks: AppointmentBlock[] = [];

    get pendingBlocks(): AppointmentBlock[] {
        return this.blocks.filter(b => b.meta.pending);
    }

    @Mutation
    SET_VIEW(view: CalendarView) {
        this.view = view;
    }

    @Mutation
    SET_DATE(date: Date) {
        this.date = date;
    }

    @Mutation
    ADD_BLOCK(block: AppointmentBlock) {
        this.blocks.push(block);
    }

    @Mutation
    SET_BLOCKS(blocks: AppointmentBlock[]) {
        this.blocks = blocks;
    }

    @Mutation
    RESIZE_BLOCK({ block, time, duration }: { block: AppointmentBlock; time: number; duration: number }) {
        block.time = time;
        block.duration = duration;

        this.blocks = [...this.blocks.filter(b => b != block), block];
    }

    @Mutation
    public SET_MODIFY_FLAG(block: AppointmentBlock) {
        block.meta.modifying = true;
        this.blocks = [...this.blocks.filter(b => b != block), block];
    }

    @Mutation
    REMOVE_MODIFY_FLAG(block: AppointmentBlock) {
        block.meta.modifying = false;
        this.blocks = [...this.blocks.filter(b => b != block), block];
    }

    @Action({ rawError: true })
    changeView(view: CalendarView) {
        this.context.commit('SET_VIEW', view);

        let m = moment(this.date);

        switch (view) {
            case 'week':
                m = m.startOf('week');
                break;
            case 'month':
                m = m.startOf('month');
                break;
        }

        this.context.commit('SET_DATE', m.toDate());
    }

    /**
     * Jump to a specific date.
     */
    @Action({ rawError: true })
    jumpDate(date: Date) {
        switch (this.view) {
            case 'week':
                date = TimeUtils.getFirstDateOfWeek(date);
                break;
            case 'month':
                date = TimeUtils.getFirstOfMonth(date);
                break;
        }

        this.context.commit('SET_DATE', date);
    }

    /**
     * Adjust the current date using a relative step.
     */
    @Action({ rawError: true })
    adjustDate({ direction, step }: { direction: 'previous' | 'next'; step: 'day' | 'week' | 'month' }) {
        let m = moment(this.date);

        switch (direction) {
            case 'next':
                m.add(1, step);
                break;

            case 'previous':
                m.subtract(1, step);
                break;
        }

        switch (this.view) {
            case 'week':
                m = m.startOf('week');
                break;
            case 'month':
                m = m.startOf('month');
                break;
        }

        this.context.commit('SET_DATE', m.toDate());
    }
}
