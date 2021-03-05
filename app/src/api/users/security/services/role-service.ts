import { http } from '@/api/shared/http';
import { Role } from '../data-transfer-objects/role';
import { RoleCreateRequest } from '../data-transfer-objects/requests/role-create-request';
import { RoleUpdateRequest } from '../data-transfer-objects/requests/role-update-request';
import { Permission } from '../data-transfer-objects/permission';

export class RoleService {
    async getRoles() {
        const res = await http.get<any[]>('/security/roles');
        return res.data.map(d => this._map(d));
    }

    async createRole(create: RoleCreateRequest) {
        const res = await http.post('/security/roles', create);
        return this._map(res.data);
    }

    async updateRole(update: RoleUpdateRequest) {
        const res = await http.put(`/security/roles/${update.id}`, update);
        return this._map(res.data);
    }

    async deleteRole(role: Role) {
        await http.delete(`/security/roles/${role.id}`);
    }

    _map(data: any) {
        // An empty array is turned to null in axios.
        const role = new Role(
            data.id,
            data.name,
            (data.permissions ?? ([] as any[])).map((p: any) => new Permission(p.id, p.action, p.scope))
        );
        return role;
    }
}

export const roleService = new RoleService();
