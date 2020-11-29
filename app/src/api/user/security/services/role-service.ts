import { http } from '@/api/core/http';
import { Role } from '../data-transfer-objects/role';
import { RoleCreateRequest } from '../data-transfer-objects/requests/role-create-request';
import { RoleUpdateRequest } from '../data-transfer-objects/requests/role-update-request';
import { Permission } from '../data-transfer-objects/permission';

export class RoleService {
    async getRoles() {
        const res = await http.get<any[]>('/security/role');
        return res.data.map(d => this._map(d));
    }

    async createRole(create: RoleCreateRequest) {
        const res = await http.post('/security/role', create);
        return this._map(res.data);
    }

    async updateRole(update: RoleUpdateRequest) {
        const res = await http.put(`/security/role/${update.id}`, update);
        return this._map(res.data);
    }

    async deleteRole(role: Role) {
        await http.delete(`/security/role/${role.id}`);
    }

    _map(data: any) {
        // An empty array is turned to null in axios.
        const role = new Role(data.id, data.name, (data.permissions ?? [] as any[]).map((p: any) => new Permission(p.id, p.action, p.scope)));
        return role;
    }
}
