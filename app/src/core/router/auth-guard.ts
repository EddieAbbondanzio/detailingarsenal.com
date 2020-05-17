import { NavigationGuard } from 'vue-router';
import Vue from 'vue';
import userStore from '@/modules/app/store/user/user-store';

export const authGuard: NavigationGuard = async (to, from, next) => {
    // Ensure auth service has been initialized
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
