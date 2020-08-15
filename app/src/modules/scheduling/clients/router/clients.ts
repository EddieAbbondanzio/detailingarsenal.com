import { authGuard } from '@/core/router/auth-guard';

export const clients = [
    {
        path: 'scheduling/clients',
        name: 'clients',
        component: () => import('@/modules/scheduling/clients/views/clients.vue')
    },
    {
        path: 'scheduling/client/create',
        name: 'createClient',
        component: () => import('@/modules/scheduling/clients/views/create-client.vue')
    },
    {
        path: 'scheduling/clients/:id',
        name: 'client',
        component: () => import('@/modules/scheduling/clients/views/client.vue')
    },
    {
        path: 'scheduling/clients/:id/edit',
        name: 'editClient',
        component: () => import('@/modules/scheduling/clients/views/edit-client.vue')
    }
];
