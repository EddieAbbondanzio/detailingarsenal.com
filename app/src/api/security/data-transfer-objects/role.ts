export class Role {
    constructor(public id: string, public name: string, public permissionIds: string[] = []) {}
}
