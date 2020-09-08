import { Module, VuexModule, Mutation, Action, getModule } from 'vuex-module-decorators';
import { api } from '@/api/api';
import { InitableModule } from '@/core/store/initable-module';
import store from '@/core/store/index';
import { Route, RawLocation } from 'vue-router';
import router from '@/core/router';
import { User } from '@/api';

@Module({ namespaced: true, name: 'user', dynamic: true, store })
class UserStore extends InitableModule {
    isAuthenticated: boolean = false;
    isLoading: boolean = true; // Used to show loading screen
    user: User = null!;

    @Mutation
    SET_IS_AUTHENTICATED(isAuthenticated: boolean) {
        this.isAuthenticated = isAuthenticated;
    }

    @Mutation
    SET_IS_LOADING(isLoading: boolean) {
        this.isLoading = isLoading;
    }

    @Mutation
    SET_USER(user: User) {
        this.user = user;
    }

    @Action({ rawError: true })
    async _init() {
        await api.authentication.init();

        if (api.authentication.isAuthenticated) {
            const user = await api.user.getUser();
            this.context.commit('SET_USER', user);
        }

        this.context.commit('SET_IS_AUTHENTICATED', api.authentication.isAuthenticated);
        this.context.commit('SET_IS_LOADING', false);
    }

    @Action({ rawError: true })
    async login(route: RawLocation | null = null) {
        await api.authentication.login(route);
    }

    @Action({ rawError: true })
    async signUp(route: RawLocation | null = null) {
        await api.authentication.signUp(route);
    }

    @Action({ rawError: true })
    async logout() {
        await api.authentication.logout();
    }

    @Action({ rawError: true })
    async updateUser(update: { name: string }) {
        const u = await api.user.updateUser(update);
        this.context.commit('SET_USER', u);
    }
}

export default getModule(UserStore);
