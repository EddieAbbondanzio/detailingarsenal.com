import { NavigationGuard } from 'vue-router';
import Vue from 'vue';
import userStore from '@/modules/user/core/store/user-store';

export const adminGuard: NavigationGuard = async (to, from, next) => {
    await userStore.init();
    const v = new Vue();

    const fn = () => {
        if (userStore.isAuthenticated) {
            if (userStore.user.isAdmin) {
                return next();
            } else {
                // Redirect to root if user is not an admin
                return next('/');
            }
        } else {
            userStore.login(to.path);
        }
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
