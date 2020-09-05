import { calendar } from '../../calendar/router/calendar';
import { settings } from '../../settings/router/settings';
import { profileRoutes } from '@/modules/user/profile/router/profile-routes';
import { clients } from '../../clients/router/clients';
import { authGuard } from '@/core/router/auth-guard';
import { RouteConfig } from 'vue-router';

export const schedulingRoutes: RouteConfig[] = [
    {
        path: '/scheduling',
        component: () => import('@/modules/scheduling/core/views/scheduling-parent-view.vue'),
        children: [...calendar, ...settings, ...profileRoutes, ...clients],
        beforeEnter: authGuard
    }
];