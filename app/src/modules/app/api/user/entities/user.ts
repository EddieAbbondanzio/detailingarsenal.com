import { Entity } from '@/core';

export class User extends Entity {
    constructor(public email: string, public name?: string) {
        super();
    }
}
