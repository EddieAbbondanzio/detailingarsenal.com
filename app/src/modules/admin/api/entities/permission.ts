import { Entity } from '@/core';

export class Permission extends Entity {
    get label() {
        return `${this.action}:${this.scope}`;
    }

    constructor(public action: string, public scope: string) {
        super();
    }
}
