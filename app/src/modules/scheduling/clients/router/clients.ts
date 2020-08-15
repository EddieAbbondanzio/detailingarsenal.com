import { authGuard } from '@/core/router/auth-guard';

export const clients = [
    {
        path: 'clients',
        name: 'clients',
        component: () => import('@/modules/scheduling/clients/views/clients.vue')
    },
    {
        path: 'client/create',
        name: 'createClient',
        component: () => import('@/modules/scheduling/clients/views/create-client.vue')
    },
    {
        path: 'clients/:id',
        name: 'client',
        component: () => import('@/modules/scheduling/clients/views/client.vue')
    },
    {
        path: 'clients/:id/edit',
        name: 'editClient',
        component: () => import('@/modules/scheduling/clients/views/edit-client.vue')
    }
];
