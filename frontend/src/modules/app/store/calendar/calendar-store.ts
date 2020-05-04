import { Module, Mutation, Action, getModule } from 'vuex-module-decorators';
import { InitableModule } from '@/core/store/initable-module';
import { CalendarView } from '@/modules/app/store/calendar/calendar-view';
import moment from 'moment';
import { TimeUtils, toast, twelveHourFormat } from '@/core';
import {
    AppointmentBlock,
    BLOCK_PENDING_FLAG,
    BLOCK_MODIFIED,
    BLOCK_INITIAL_TIME
} from '@/modules/app/api/calendar/entities/appointment-block';
import store from '@/core/store/index';
import { CalendarCreateStep } from '@/modules/app/store/calendar/calendar-create-step';
import { CreateAppointment, api } from '@/modules/app/api';
import { CalendarRange } from '@/modules/app/store/calendar/calendar-range';
import { displayError } from '@/modules/app/utils/display-error/display-error';
import Vue from 'vue';
import { Appointment } from '@/modules/app/api/calendar/entities/appointment';

/**
 * Store for the Calendar view.
 */
@Module({ namespaced: true, name: 'calendar', store, dynamic: true })
class CalendarStore extends InitableModule {
    view: CalendarView = 'day';
    date: Date = new Date(new Date().toDateString());
    createStep: CalendarCreateStep | null = null;

    blocks: AppointmentBlock[] = [];

    active: Appointment | null = null;

    get pendingBlocks(): AppointmentBlock[] {
        return this.blocks.filter(b => b.meta.pending).sort((a, b) => (a.start < b.start ? -1 : 1));
    }

    get hasBlockForDateTime() {
        return (dateTime: Date) => this.blocks.some(b => moment(b.start).isSame(dateTime, 'minutes'));
    }

    get blockForDateTime() {
        return (dateTime: Date) => this.blocks.find(b => moment(b.start).isSame(dateTime, 'minutes'));
    }

    get blocksForDay() {
        return (date: Date) => this.blocks.filter(b => moment(b.start).isSame(date, 'day'));
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
    DELETE_BLOCKS_FOR(appointment: Appointment) {
        this.blocks = this.blocks.filter(b => b.appointment.id != appointment.id);
    }

    @Mutation
    CLEAR_NONPENDING_BLOCKS() {
        this.blocks = this.blocks.filter(b => b.meta[BLOCK_PENDING_FLAG]);
    }

    @Mutation
    CLEAR_PENDING_BLOCKS() {
        this.blocks = this.blocks.filter(b => !b.meta[BLOCK_PENDING_FLAG]);
    }

    @Mutation
    DELETE_BLOCK(block: AppointmentBlock) {
        this.blocks = [...this.blocks.filter(b => b != block)];
    }

    @Mutation
    ADD_BLOCK_META({ block, meta }: { block: AppointmentBlock; meta: { name: string; value: any } }) {
        Vue.set(block.meta, meta.name, meta.value);
    }

    @Mutation
    REMOVE_BLOCK_META({ block, name }: { block: AppointmentBlock; name: string }) {
        Vue.set(block.meta, name, undefined);
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
        Vue.set(block, 'start', start);
    }

    @Mutation
    UPDATE_BLOCK_END({ block, end }: { block: AppointmentBlock; end: Date }) {
        Vue.set(block, 'end', end);
    }

    @Mutation
    SET_ACTIVE_APPOINTMENT(appointment: Appointment) {
        this.active = appointment;
    }

    @Mutation
    CLEAR_ACTIVE_APPOINTMENT() {
        this.active = null;
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
        this.context.commit('CLEAR_NONPENDING_BLOCKS');

        this.context.commit(
            'ADD_BLOCKS',
            appointments.flatMap(a => a.blocks)
        );
    }

    @Action({ rawError: true })
    async createAppointment(create: CreateAppointment) {
        let a = await api.appointment.createAppointment(create);
        this.context.commit('ADD_BLOCKS', a.blocks);
        return a;
    }

    @Action({ rawError: true })
    async deleteAppointment(appointment: Appointment) {
        await api.appointment.deleteAppointment(appointment.id);
        this.context.commit('DELETE_BLOCKS_FOR', appointment);
    }

    @Action({ rawError: true })
    async saveBlockChanges(block: AppointmentBlock) {
        // Save off changes to backend, if the block isn't pending, and actually has changes
        if (!block.meta[BLOCK_PENDING_FLAG] && block.meta[BLOCK_MODIFIED]) {
            try {
                await api.appointment.updateAppointment(block.appointment);
                toast(`Updated appointment`);

                this.context.commit('REMOVE_BLOCK_META', {
                    block,
                    name: BLOCK_MODIFIED
                });

                this.context.commit('REMOVE_BLOCK_META', {
                    block,
                    name: BLOCK_INITIAL_TIME
                });
            } catch (err) {
                console.log(err);
                displayError(err);
            }
        }
    }

    @Action({ rawError: true })
    async cancelPendingChanges() {
        this.context.commit('CLEAR_PENDING_BLOCKS');
        this.context.commit('CLEAR_CREATE_STEP');
    }
}

export default getModule(CalendarStore);
