import { Client } from '@/modules/clients/api/entities/client';
import { CreateClient } from '@/modules/clients/api/data-transfer-objects/create-client';
import { UpdateClient } from '@/modules/clients/api/data-transfer-objects/update-client';
import { http } from '@/core';

export class ClientService {
    async getClients(): Promise<Client[]> {
        const res = await http.get<any[]>('/client');
        return res.data.map(r => this._map(r));
    }

    async createClient(create: CreateClient): Promise<Client> {
        const res = await http.post('/client', create);
        return this._map(res.data);
    }

    async updateClient(update: UpdateClient): Promise<Client> {
        const res = await http.put(`/client/${update.id}`, update);
        return this._map(res.data);
    }

    async deleteClient(client: Client): Promise<void> {
        await http.delete(`client/${client.id}`);
    }

    _map(a: any): Client {
        var c = new Client(a.name, a.phone, a.email);
        c.id = a.id;

        return c;
    }
}
