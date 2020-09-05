import { calendar } from '../../calendar/router/calendar';
import { settings } from '../../settings/router/settings';
import { clients } from '../../clients/router/clients';
import { authGuard } from '@/core/router/auth-guard';
import { RouteConfig } from 'vue-router';
import { subscriptionRoutes } from '../../subscription/router/subscription-routes';

export const schedulingRoutes: RouteConfig[] = [
    {
        path: '/scheduling',
        component: () => import('@/modules/scheduling/core/views/scheduling-parent-view.vue'),
        children: [...calendar, ...settings, ...clients, ...subscriptionRoutes],
        beforeEnter: authGuard
    }
];