import { http } from '@/api/core/http';
import { User } from '../data-transfer-objects/user';
import { UserPermission } from '../data-transfer-objects/user-permission';

export class UserService {
    async getUser(): Promise<User> {
        const res = await http.get('users');

        var perms = Array.isArray(res.data.permissions)
            ? (res.data.permissions as any[]).map(p => new UserPermission(p.action, p.scope))
            : [];

        return new User(
            res.data.id,
            res.data.username,
            res.data.email,
            res.data.name,
            res.data.joinedDate,
            res.data.isAdmin,
            perms
        );
    }

    async updateUser(update: { name: string }) {
        var res = await http.put(`users`, update);
        return await this.getUser();
    }
}
