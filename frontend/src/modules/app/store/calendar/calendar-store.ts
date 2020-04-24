import { Module, Mutation, Action, getModule } from 'vuex-module-decorators';
import { InitableModule } from '@/core/store/initable-module';
import { CalendarView } from '@/modules/app/store/calendar/calendar-view';
import moment from 'moment';
import { TimeUtils } from '@/core';
import { AppointmentBlock } from '@/modules/app/api/calendar/entities/appointment-block';
import store from '@/core/store/index';
import { CalendarCreateStep } from '@/modules/app/store/calendar/calendar-create-step';
import { CreateAppointment, api } from '@/modules/app/api';
import { CalendarRange } from '@/modules/app/store/calendar/calendar-range';

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
        return this.blocks.filter(b => b.meta.pending).sort((a, b) => (a.start < b.start ? -1 : 1));
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
    ADD_BLOCKS(blocks: AppointmentBlock[]) {
        this.blocks.push(...blocks);
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
    UPDATE_BLOCK_START({ block, start }: { block: AppointmentBlock; start: Date }) {
        block.start = start;
        this.blocks = [...this.blocks.filter(b => b != block), block];
    }

    @Mutation
    UPDATE_BLOCK_END({ block, end }: { block: AppointmentBlock; end: Date }) {
        block.end = end;
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

    @Action({ rawError: true })
    async loadAppointments({ date, range }: { date: Date; range: CalendarRange }) {
        const appointments = await api.appointment.get(date, range);
        console.log(appointments);
    }

    @Action({ rawError: true })
    async createAppointment(create: CreateAppointment) {
        let a = await api.appointment.createAppointment(create);
        this.context.commit('ADD_BLOCKS', a.blocks);
        return a;
    }
}

export default getModule(CalendarStore);
