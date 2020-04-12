import { Module, VuexModule, Mutation, Action } from 'vuex-module-decorators';
import { InitableModule } from '@/core/store/initable-module';
import { CalendarView } from '@/modules/app/store/calendar/calendar-view';
import moment from 'moment';
import { TimeUtils } from '@/core';

/**
 * Store for the Calendar view.
 */
@Module({ namespaced: true, name: 'calendar' })
export default class CalendarStore extends InitableModule {
    view: CalendarView = 'day';
    date: Date = new Date();

    @Mutation
    SET_VIEW(view: CalendarView) {
        this.view = view;
    }

    @Mutation
    SET_DATE(date: Date) {
        this.date = date;
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
