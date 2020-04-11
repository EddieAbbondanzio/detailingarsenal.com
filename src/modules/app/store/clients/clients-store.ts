import { Module, VuexModule, Mutation, Action } from 'vuex-module-decorators';
import { Client } from '@/modules/app/api/clients/entities/client';
import { InitableModule } from '@/core/store/initable-module';
import { CreateClient } from '@/modules/app/api/clients/data-transfer-objects/create-client';
import { UpdateClient } from '@/modules/app/api/clients/data-transfer-objects/update-client';
import { api } from '@/modules/app/api/api';

/**
 * Store for the schedule view.
 */
@Module({ namespaced: true, name: 'clients' })
export default class ClientsStore extends InitableModule {
    clients: Client[] = [];

    @Mutation
    SET_CLIENTS(clients: Client[]) {
        this.clients = clients;
    }

    @Mutation
    CREATE_CLIENT(client: Client) {
        this.clients.push(client);
    }

    @Mutation
    UPDATE_CLIENT(client: Client) {
        this.clients = [...this.clients.filter(c => c.id != client.id), client];
    }

    @Mutation
    DELETE_CLIENT(client: Client) {
        const index = this.clients.findIndex(c => c.id == client.id);
        this.clients.splice(index, 1);
    }

    @Action({ rawError: true })
    async _init() {
        const clients = await api.client.getClients();
        this.context.commit('SET_CLIENTS', clients);
    }

    @Action({ rawError: true })
    public async createClient(create: CreateClient): Promise<Client> {
        const c = await api.client.createClient(create);
        this.context.commit('CREATE_CLIENT', c);
        return c;
    }

    @Action({ rawError: true })
    public async updateClient(update: UpdateClient): Promise<Client> {
        const c = await api.client.updateClient(update);
        this.context.commit('UPDATE_CLIENT', c);
        return c;
    }

    @Action({ rawError: true })
    public async deleteClient(client: Client) {
        await api.client.deleteClient(client);
        this.context.commit('DELETE_CLIENT', client);
    }
}
