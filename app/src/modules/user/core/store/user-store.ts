import { Module, VuexModule, Mutation, Action, getModule } from 'vuex-module-decorators';
import { InitableModule } from '@/core/store/initable-module';
import store from '@/core/store/index';
import { Route, RawLocation } from 'vue-router';
import router from '@/core/router';
import { AuthenticationService, User, UserService } from '@/api/users';

@Module({ namespaced: true, name: 'user', dynamic: true, store })
class UserStore extends InitableModule {
    isAuthenticated: boolean = false;
    isLoading: boolean = true; // Used to show loading screen
    user: User = null!;

    private authenticationService: AuthenticationService = new AuthenticationService();
    private userService: UserService = new UserService();

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
        await this.authenticationService.init();

        if (this.authenticationService.isAuthenticated) {
            const user = await this.userService.getUser();
            this.context.commit('SET_USER', user);
        }

        this.context.commit('SET_IS_AUTHENTICATED', this.authenticationService.isAuthenticated);
        this.context.commit('SET_IS_LOADING', false);
    }

    @Action({ rawError: true })
    async login(route: RawLocation | null = null) {
        await this.authenticationService.login(route);
    }

    @Action({ rawError: true })
    async signUp(route: RawLocation | null = null) {
        await this.authenticationService.signUp(route);
    }

    @Action({ rawError: true })
    async logout() {
        await this.authenticationService.logout();
    }

    @Action({ rawError: true })
    async updateUser(update: { name: string }) {
        const u = await this.userService.updateUser(update);
        this.context.commit('SET_USER', u);
    }
}

export default getModule(UserStore);
