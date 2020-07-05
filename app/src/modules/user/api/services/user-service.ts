import { User } from '@/modules/user/api/entities/user';
import { http } from '@/core/api/http';

export class UserService {
    async getUser(): Promise<User> {
        const res = await http.get('user');
        return new User(res.data.email, res.data.name);
    }

    async updateUser(update: { name: string }) {
        var res = await http.put(`/user`, update);
        return await this.getUser();
    }
}
