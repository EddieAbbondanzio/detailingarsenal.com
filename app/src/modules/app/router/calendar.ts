import { authGuard } from '@/core/router/auth-guard';

export const calendar = [
    {
        path: 'calendar',
        name: 'calendar',
        component: () => import('@/modules/app/views/calendar/calendar.vue')
    }
];
