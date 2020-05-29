import { Entity } from '@/core';

export class Permission extends Entity {
    constructor(public action: string, public scope: string) {
        super();
    }

    toString() {
        return `${this.action}:${this.scope}`;
    }
}
