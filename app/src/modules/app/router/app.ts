import { settings } from '@/modules/app/router/settings';
import { calendar } from '@/modules/app/router/calendar';
import { user } from '@/modules/app/router/user';
import { clients } from '@/modules/app/router/clients';
import { authGuard } from '@/core/router/auth-guard';

export const app = {
    path: '/app',
    component: () => import('@/modules/app/views/app.vue'),
    children: [...calendar, ...settings, ...user, ...clients],
    beforeEnter: authGuard
};
