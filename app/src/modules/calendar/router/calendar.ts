import { authGuard } from '@/core/router/auth-guard';

export const calendar = [
    {
        path: 'calendar',
        name: 'calendar',
        component: () => import('@/modules/calendar/views/calendar.vue')
    }
];
