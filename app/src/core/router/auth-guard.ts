import { NavigationGuard } from 'vue-router';
import Vue from 'vue';
import userStore from '@/modules/scheduling/user/store/user-store';

export const authGuard: NavigationGuard = async (to, from, next) => {
    const v = new Vue();

    const fn = () => {
        if (userStore.isAuthenticated) {
            return next();
        } else {
            userStore.login(to);
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
