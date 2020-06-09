import { CreatePermission } from '@/modules/admin/api/data-transfer-objects/create-permission';
import { UpdatePermission } from '@/modules/admin/api/data-transfer-objects/update-permission';
import { Permission } from '@/modules/admin/api/entities/permission';
import { http } from '@/core';

export class PermissionService {
    async getPermissions() {
        const res = await http.get<any[]>('/security/permission');
        return res.data.map(r => this._map(r));
    }

    async createPermission(create: CreatePermission) {
        const res = await http.post('/security/permission', create);
        return this._map(res.data);
    }

    async updatePermission(update: UpdatePermission) {
        const res = await http.put(`/security/permission/${update.id}`, update);
        return this._map(res.data);
    }

    async deletePermission(permission: Permission) {
        await http.delete(`/security/permission/${permission.id}`);
    }

    _map(data: any) {
        const p = new Permission(data.action, data.scope);
        p.id = data.id;

        return p;
    }
}
