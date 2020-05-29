import { CreatePermission } from '@/modules/admin/api/data-transfer-objects/create-permission';
import { UpdatePermission } from '@/modules/admin/api/data-transfer-objects/update-permission';
import { Permission } from '@/modules/admin/api/entities/permission';
import { http } from '@/core';

export class PermissionService {
    async getPermissions() {
        const res = await http.get<any[]>('/authorization/permission');
        return res.data.map(r => this._map(r));
    }

    async createPermission(create: CreatePermission) {
        const res = await http.post('/authorization/permission', create);
        return this._map(res.data);
    }

    async updatePermission(update: UpdatePermission) {
        const res = await http.post(`/authorization/permission/${update.id}`, update);
        return this._map(res.data);
    }

    async deletePermission(id: string) {
        await http.delete(`/authorization/permission/${id}`);
    }

    _map(data: any) {
        const p = new Permission(data.action, data.scope);
        p.id = data.id;

        return p;
    }
}
