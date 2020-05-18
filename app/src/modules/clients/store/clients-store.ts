import { Module, VuexModule, Mutation, Action, getModule } from 'vuex-module-decorators';
import { Client } from '@/modules/clients/api/entities/client';
import { InitableModule } from '@/core/store/initable-module';
import { CreateClient } from '@/modules/clients/api/data-transfer-objects/create-client';
import { UpdateClient } from '@/modules/clients/api/data-transfer-objects/update-client';
import store from '@/core/store/index';
import { api } from '@/core/api/api';

/**
 * Store for the schedule view.
 */
@Module({ namespaced: true, name: 'clients', dynamic: true, store })
class ClientsStore extends InitableModule {
    clients: Client[] = [];

    get search() {
        return (name: string) => this.clients.filter(c => c.name.toLowerCase().includes(name.toLowerCase()));
    }

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

export default getModule(ClientsStore);
