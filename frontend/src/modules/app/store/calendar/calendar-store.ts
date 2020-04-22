import { Module, Mutation, Action, getModule } from 'vuex-module-decorators';
import { InitableModule } from '@/core/store/initable-module';
import { CalendarView } from '@/modules/app/store/calendar/calendar-view';
import moment from 'moment';
import { TimeUtils } from '@/core';
import { AppointmentBlock } from '@/modules/app/api/calendar/entities/appointment-block';
import store from '@/core/store/index';
import { CalendarCreateStep } from '@/modules/app/store/calendar/calendar-create-step';

/**
 * Store for the Calendar view.
 */
@Module({ namespaced: true, name: 'calendar', store, dynamic: true })
class CalendarStore extends InitableModule {
    view: CalendarView = 'day';
    date: Date = new Date(new Date().toDateString());
    createStep: CalendarCreateStep | null = null;

    blocks: AppointmentBlock[] = [];

    get pendingBlocks(): AppointmentBlock[] {
        return this.blocks.filter(b => b.meta.pending).sort((a, b) => (a.date < b.date ? -1 : 1));
    }

    @Mutation
    SET_VIEW(view: CalendarView) {
        this.view = view;
    }

    @Mutation
    SET_DATE(date: Date) {
        this.date = new Date(date.toDateString());
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
    CLEAR_BLOCKS() {
        this.blocks = [];
    }

    @Mutation
    RESIZE_BLOCK({ block, time, duration }: { block: AppointmentBlock; time: number; duration: number }) {
        block.time = time;
        block.duration = duration;

        this.blocks = [...this.blocks.filter(b => b != block), block];
    }

    @Mutation
    MOVE_BLOCK({ block, time }: { block: AppointmentBlock; time: number }) {
        block.time = time;
        this.blocks = [...this.blocks.filter(b => b != block), block];
    }

    @Mutation
    DELETE_BLOCK(block: AppointmentBlock) {
        this.blocks = [...this.blocks.filter(b => b != block)];
    }

    @Mutation
    ADD_BLOCK_META({ block, meta }: { block: AppointmentBlock; meta: { name: string; value: any } }) {
        block.meta[meta.name] = meta.value;
        this.blocks = [...this.blocks.filter(b => b != block), block];
    }

    @Mutation
    REMOVE_BLOCK_META({ block, name }: { block: AppointmentBlock; name: string }) {
        block.meta[name] = undefined;
        this.blocks = [...this.blocks.filter(b => b != block), block];
    }

    @Mutation
    SET_CREATE_STEP(step: CalendarCreateStep) {
        this.createStep = step;
    }

    @Mutation
    CLEAR_CREATE_STEP() {
        this.createStep = null;
    }

    @Mutation
    UPDATE_BLOCK_DATE({ block, date }: { block: AppointmentBlock; date: Date }) {
        block.date = date;
        this.blocks = [...this.blocks.filter(b => b != block), block];
    }

    @Mutation
    UPDATE_BLOCK_TIME({ block, time }: { block: AppointmentBlock; time: number }) {
        block.time = time;
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

export default getModule(CalendarStore);
