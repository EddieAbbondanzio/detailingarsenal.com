import { authGuard } from '@/core/router/auth-guard';

export const calendar = [
    {
        path: 'calendar',
        name: 'calendar',
        component: () => import('@/modules/scheduling/calendar/views/calendar.vue')
    }
];
