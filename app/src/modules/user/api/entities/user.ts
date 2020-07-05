import { Entity } from '@/core';
import { UserPermission } from '@/modules/user/api/entities/user-permission';

export class User extends Entity {
    constructor(
        public email: string,
        public name: string | null,
        public isAdmin: boolean,
        public permissions: UserPermission[]
    ) {
        super();
    }

    /**
     * Check to see if a user has a specific permission.
     * @param action The action to check.
     * @param scope The scope to check.
     */
    hasPermission(action: string, scope: string) {
        return this.permissions.some(p => p.action == action && p.scope == scope);
    }
}
