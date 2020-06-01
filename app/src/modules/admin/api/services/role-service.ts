import { http } from '@/core';
import { Role } from '@/modules/admin/api/entities/role';
import { CreateRole } from '@/modules/admin/api/data-transfer-objects/create-role';
import { UpdateRole } from '@/modules/admin/api/data-transfer-objects/update-role';

export class RoleService {
    async getRoles() {
        const res = await http.get<any[]>('/authorization/role');
        return res.data.map(d => this._map(d));
    }

    async createRole(create: CreateRole) {
        const res = await http.post('/authorization/role', create);
        return this._map(res.data);
    }

    async updateRole(update: UpdateRole) {
        const res = await http.put(`/authorization/role/${update.id}`, update);
        return this._map(res.data);
    }

    async deleteRole(role: Role) {
        await http.delete(`/authorization/role/${role.id}`);
    }

    _map(data: any) {
        const role = new Role(data.name, data.permissionIds);
        role.id = data.id;

        return role;
    }
}