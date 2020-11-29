import { http } from '@/api/core/http';
import { Role } from '../data-transfer-objects/role';
import { RoleCreate } from '../data-transfer-objects/role-create';
import { RoleUpdate } from '../data-transfer-objects/role-update';

export class RoleService {
    async getRoles() {
        const res = await http.get<any[]>('/security/role');
        return res.data.map(d => this._map(d));
    }

    async createRole(create: RoleCreate) {
        const res = await http.post('/security/role', create);
        return this._map(res.data);
    }

    async updateRole(update: RoleUpdate) {
        const res = await http.put(`/security/role/${update.id}`, update);
        return this._map(res.data);
    }

    async deleteRole(role: Role) {
        await http.delete(`/security/role/${role.id}`);
    }

    _map(data: any) {
        const role = new Role(data.id, data.name, data.permissionIds);
        return role;
    }
}
