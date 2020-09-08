import { http } from '@/api/core/http';
import { User } from '@/api/user/data-transfer-objects/user';
import { UserPermission } from '@/api/user/data-transfer-objects/user-permission';

export class UserService {
    async getUser(): Promise<User> {
        const res = await http.get('user');

        var perms = Array.isArray(res.data.permissions)
            ? (res.data.permissions as any[]).map(p => new UserPermission(p.action, p.scope))
            : [];

        return new User(res.data.id, res.data.email, res.data.name, res.data.joinedDate, res.data.isAdmin, perms);
    }

    async updateUser(update: { name: string }) {
        var res = await http.put(`/user`, update);
        return await this.getUser();
    }
}
