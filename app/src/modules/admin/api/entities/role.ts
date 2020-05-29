import { Entity } from '@/core';
import { Permission } from '@/modules/admin/api/entities/permission';

export class Role extends Entity {
    constructor(public name: string, public permissions: Permission[]) {
        super();
    }
}
