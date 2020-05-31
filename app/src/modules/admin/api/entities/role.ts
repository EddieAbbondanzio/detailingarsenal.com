import { Entity } from '@/core';

export class Role extends Entity {
    constructor(public name: string, public permissionIds: number[]) {
        super();
    }
}
