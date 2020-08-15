import { PermissionCreate } from '@/api/scheduling/security/data-transfer-objects/permission-create';
import { PermissionUpdate } from '@/api/scheduling/security/data-transfer-objects/permission-update';
import { Permission } from '@/api/scheduling/security/data-transfer-objects/permission';
import { http } from '@/api/core/http';

export class PermissionService {
    async getPermissions() {
        const res = await http.get<any[]>('/security/permission');
        return res.data.map(r => this._map(r));
    }

    async createPermission(create: PermissionCreate) {
        const res = await http.post('/security/permission', create);
        return this._map(res.data);
    }

    async updatePermission(update: PermissionUpdate) {
        const res = await http.put(`/security/permission/${update.id}`, update);
        return this._map(res.data);
    }

    async deletePermission(permission: Permission) {
        await http.delete(`/security/permission/${permission.id}`);
    }

    _map(data: any) {
        const p = new Permission(data.id, data.action, data.scope);
        return p;
    }
}
