import { http } from '@/api/shared/http';
import { Permission } from '../data-transfer-objects/permission';
import { PermissionCreateRequest } from '../data-transfer-objects/requests/permission-create-request';
import { PermissionUpdateReqest } from '../data-transfer-objects/requests/permission-update-request';

export class PermissionService {
    async getAll() {
        const res = await http.get<any[]>('/security/permissions');
        return res.data.map(r => this._map(r));
    }

    async createPermission(create: PermissionCreateRequest) {
        const res = await http.post('/security/permissions', create);
        return this._map(res.data);
    }

    async updatePermission(update: PermissionUpdateReqest) {
        const res = await http.put(`/security/permissions/${update.id}`, update);
        return this._map(res.data);
    }

    async deletePermission(permission: Permission) {
        await http.delete(`/security/permissions/${permission.id}`);
    }

    _map(data: any) {
        const p = new Permission(data.id, data.action, data.scope);
        return p;
    }
}

export const permissionService = new PermissionService();
