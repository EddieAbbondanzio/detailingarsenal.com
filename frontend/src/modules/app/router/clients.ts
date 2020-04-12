import { authGuard } from '@/core/router/auth-guard';

export const clients = [
    {
        path: 'clients',
        name: 'clients',
        component: () => import('@/modules/app/views/clients/clients.vue')
    },
    {
        path: 'client/create',
        name: 'createClient',
        component: () => import('@/modules/app/views/clients/create-client.vue')
    },
    {
        path: 'clients/:id',
        name: 'client',
        component: () => import('@/modules/app/views/clients/client.vue')
    },
    {
        path: 'clients/:id/edit',
        name: 'editClient',
        component: () => import('@/modules/app/views/clients/edit-client.vue')
    }
];
