import { User } from '@/modules/user/api/entities/user';
import { http } from '@/core/api/http';
import { UserPermission } from '@/modules/user/api/entities/user-permission';

export class UserService {
    async getUser(): Promise<User> {
        const res = await http.get('user');
        return new User(
            res.data.email,
            res.data.name,
            res.data.isAdmin,
            (res.data.permissions as any[]).map(p => new UserPermission(p.action, p.scope))
        );
    }

    async updateUser(update: { name: string }) {
        var res = await http.put(`/user`, update);
        return await this.getUser();
    }
}
