import { Module, VuexModule, Mutation, Action } from 'vuex-module-decorators';
import { User } from '@/modules/app/api/user/entities/user';
import { api } from '@/modules/app/api/api';
import { InitableModule } from '@/core/store/initable-module';

@Module({ namespaced: true, name: 'user' })
export default class UserStore extends InitableModule {
    public isAuthenticated: boolean = false;
    public isLoading: boolean = true;
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
        await api.auth.init();

        if (api.auth.isAuthenticated) {
            const user = await api.user.getUser();
            this.context.commit('SET_USER', user);
        }

        this.context.commit('SET_IS_AUTHENTICATED', api.auth.isAuthenticated);
        this.context.commit('SET_IS_LOADING', false);
    }

    @Action({ rawError: true })
    public async login() {
        await api.auth.login();
    }

    @Action({ rawError: true })
    public async logout() {
        await api.auth.logout();
    }

    @Action({ rawError: true })
    public async updateUser(update: { name: string }) {
        var u = await api.user.updateUser(update);
        this.context.commit('SET_USER', u);
    }
}
