import Vue from 'vue';
import Vuex, { Store } from 'vuex';
import CalendarStore from '@/modules/app/store/calendar/calendar-store';
import SettingsStore from '@/modules/app/store/settings/settings-store';
import ClientsStore from '@/modules/app/store/clients/clients-store';
import UserStore from '@/modules/app/store/user/user-store';

Vue.use(Vuex);

const store = new Vuex.Store({
    state: {},
    mutations: {},
    modules: {
        calendar: CalendarStore,
        settings: SettingsStore,
        clients: ClientsStore,
        user: UserStore
    },
    actions: {},
    strict: true
});

export default store;
