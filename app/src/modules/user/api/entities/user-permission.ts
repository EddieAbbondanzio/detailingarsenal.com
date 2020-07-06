import { ValueObject } from '@/core/api/value-object';

export class UserPermission extends ValueObject {
    constructor(public action: string, public scope: string) {
        super();
    }
}
