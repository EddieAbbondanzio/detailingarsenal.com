import { NavigationGuard } from 'vue-router';
import userStore from '@/modules/scheduling/user/store/user-store';

export const adminGuard: NavigationGuard = async (to, from, next) => {
    if (userStore.user.isAdmin) {
        return next();
    } else {
        return next('/');
    }
};
