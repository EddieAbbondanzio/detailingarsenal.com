import { http } from '@/api/core/http';
import { Permission } from '../data-transfer-objects/permission';
import { PermissionCreateRequest } from '../data-transfer-objects/requests/permission-create-request';
import { PermissionUpdateReqest } from '../data-transfer-objects/requests/permission-update-request';

export class PermissionService {
    async getPermissions() {
        const res = await http.get<any[]>('/security/permission');
        return res.data.map(r => this._map(r));
    }

    async createPermission(create: PermissionCreateRequest) {
        const res = await http.post('/security/permission', create);
        return this._map(res.data);
    }

    async updatePermission(update: PermissionUpdateReqest) {
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
