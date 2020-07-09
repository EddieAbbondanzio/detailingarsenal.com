export class Permission {
    get label() {
        return `${this.action}:${this.scope}`;
    }

    constructor(public id: string, public action: string, public scope: string) {}
}
