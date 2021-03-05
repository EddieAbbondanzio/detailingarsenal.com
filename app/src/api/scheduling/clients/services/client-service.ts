import { Client } from '@/api/scheduling/clients/data-transfer-objects/client';
import { http } from '@/api/shared/http';
import { ClientCreate } from '@/api/scheduling/clients/data-transfer-objects/client-create';
import { ClientUpdate } from '@/api/scheduling/clients/data-transfer-objects/client-update';

export class ClientService {
    async getAll(): Promise<Client[]> {
        const res = await http.get<any[]>('/client');
        return res.data.map(r => this._map(r));
    }

    async createClient(create: ClientCreate): Promise<Client> {
        const res = await http.post('/client', create);
        return this._map(res.data);
    }

    async updateClient(update: ClientUpdate): Promise<Client> {
        const res = await http.put(`/client/${update.id}`, update);
        return this._map(res.data);
    }

    async deleteClient(client: Client): Promise<void> {
        await http.delete(`client/${client.id}`);
    }

    _map(a: any): Client {
        var c = new Client(a.id, a.name, a.phone, a.email);

        return c;
    }
}

export const clientService = new ClientService();
