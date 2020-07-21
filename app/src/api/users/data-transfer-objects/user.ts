import { UserPermission } from '@/api/users/data-transfer-objects/user-permission';

export class User {
    constructor(
        public id: string,
        public email: string,
        public name: string | null,
        public joinedDate: Date,
        public isAdmin: boolean,
        public permissions: UserPermission[]
    ) {}

    /**
     * Check to see if a user has a specific permission.
     * @param action The action to check.
     * @param scope The scope to check.
     */
    hasPermission(action: string, scope: string) {
        return this.permissions.some(p => p.action == action && p.scope == scope);
    }
}
