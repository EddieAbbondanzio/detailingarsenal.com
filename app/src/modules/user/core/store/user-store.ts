import { Module, VuexModule, Mutation, Action, getModule } from 'vuex-module-decorators';
import { InitableModule } from '@/core/store/initable-module';
import store from '@/core/store/index';
import { Route, RawLocation } from 'vue-router';
import router from '@/core/router';
import { authenticationService, AuthenticationService, User, userService, UserService } from '@/api/users';

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
        await authenticationService.init();

        if (authenticationService.isAuthenticated) {
            const user = await userService.getUser();
            this.context.commit('SET_USER', user);
        }

        this.context.commit('SET_IS_AUTHENTICATED', authenticationService.isAuthenticated);
        this.context.commit('SET_IS_LOADING', false);
    }

    @Action({ rawError: true })
    async login(route: RawLocation | null = null) {
        await authenticationService.login(route);
    }

    @Action({ rawError: true })
    async signUp(route: RawLocation | null = null) {
        await authenticationService.signUp(route);
    }

    @Action({ rawError: true })
    async logout() {
        await authenticationService.logout();
    }

    @Action({ rawError: true })
    async updateUser(update: { name: string }) {
        const u = await userService.updateUser(update);
        this.context.commit('SET_USER', u);
    }
}

export default getModule(UserStore);
