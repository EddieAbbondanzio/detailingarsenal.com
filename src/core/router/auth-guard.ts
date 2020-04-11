import { getModule } from 'vuex-module-decorators';
import UserStore from '@/modules/app/store/user/user-store';
import store from '@/core/store';
import { NavigationGuard } from 'vue-router';
import Vue from 'vue';

export const authGuard: NavigationGuard = async (to, from, next) => {
    // Ensure auth service has been initialized
    const userStore = getModule(UserStore, store);
    await userStore.init();

    const v = new Vue();

    const fn = () => {
        if (userStore.isAuthenticated) {
            return next();
        }

        userStore.login();
    };

    if (!userStore.isLoading) {
        return fn();
    }

    v.$watch(
        () => userStore.isLoading,
        isLoading => {
            if (!isLoading) {
                return fn();
            }
        }
    );
};
