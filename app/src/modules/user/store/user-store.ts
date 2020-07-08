import { Module, VuexModule, Mutation, Action, getModule } from 'vuex-module-decorators';
import { User } from '@/modules/user/api/entities/user';
import { api } from '@/core/api/api';
import { InitableModule } from '@/core/store/initable-module';
import store from '@/core/store/index';
import { Route } from 'vue-router';
import router from '@/core/router';
import { SubscriptionPlan } from '@/modules/admin/api/entities/subscription-plan';

@Module({ namespaced: true, name: 'user', dynamic: true, store })
class UserStore extends InitableModule {
    public isAuthenticated: boolean = false;
    public isLoading: boolean = true; // Used to show loading screen
    public user: User = null!;

    @Mutation
    public SET_IS_AUTHENTICATED(isAuthenticated: boolean) {
        this.isAuthenticated = isAuthenticated;
    }

    @Mutation
    public SET_IS_LOADING(isLoading: boolean) {
        this.isLoading = isLoading;
    }

    @Mutation
    public SET_USER(user: User) {
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
    public async login(route: Route | null = null) {
        if (route == null) {
            route = router.resolve('/calendar').route;
        }

        await api.authentication.login(route);
    }

    @Action({ rawError: true })
    public async logout() {
        await api.authentication.logout();
    }

    @Action({ rawError: true })
    public async updateUser(update: { name: string }) {
        const u = await api.user.updateUser(update);
        this.context.commit('SET_USER', u);
    }
}

export default getModule(UserStore);
