import { Module, VuexModule, Mutation, Action, getModule } from 'vuex-module-decorators';
import { InitableModule } from '@/core/store/initable-module';
import store from '@/core/store/index';
import {
    Permission,
    Role,
    PermissionCreateRequest,
    PermissionUpdateReqest as PermissionUpdateRequest,
    RoleCreateRequest,
    RoleUpdateRequest
} from '@/api/users';
import { permissionService, roleService } from '@/api/users';

/**
 * Store for the schedule view.
 */
@Module({ namespaced: true, name: 'security', dynamic: true, store })
class SecurityStore extends InitableModule {
    permissions: Permission[] = [];
    roles: Role[] = [];

    @Mutation
    SET_PERMISSIONS(permissions: Permission[]) {
        this.permissions = permissions.sort((a, b) => (a.scope > b.scope ? 1 : -1));
    }

    @Mutation
    CREATE_PERMISSION(permission: Permission) {
        this.permissions.push(permission);
        this.permissions.sort((a, b) => (a.scope > b.scope ? 1 : -1));
    }

    @Mutation
    UPDATE_PERMISSION(permission: Permission) {
        this.permissions = [...this.permissions.filter(p => p.id != permission.id), permission];
        this.permissions.sort((a, b) => (a.scope > b.scope ? 1 : -1));
    }

    @Mutation
    DELETE_PERMISSION(permission: Permission) {
        const index = this.permissions.findIndex(p => p.id == permission.id);

        if (index != -1) {
            this.permissions.splice(index, 1);
            this.permissions.sort((a, b) => (a.scope > b.scope ? 1 : -1));
        }
    }

    @Mutation
    SET_ROLES(roles: Role[]) {
        this.roles = roles;
    }

    @Mutation
    CREATE_ROLE(role: Role) {
        this.roles.push(role);
    }

    @Mutation
    UPDATE_ROLE(role: Role) {
        this.roles = [...this.roles.filter(r => r.id != role.id), role];
    }

    @Mutation
    DELETE_ROLE(role: Role) {
        const index = this.roles.findIndex(r => r.id == role.id);

        if (index != -1) {
            this.roles.splice(index, 1);
        }
    }

    @Action({ rawError: true })
    async _init() {
        const [perms, roles] = await Promise.all([permissionService.getAll(), roleService.getAll()]);

        this.context.commit('SET_PERMISSIONS', perms);
        this.context.commit('SET_ROLES', roles);
    }

    @Action({ rawError: true })
    async createPermission(createPermission: PermissionCreateRequest) {
        const p = await permissionService.createPermission(createPermission);
        this.context.commit('CREATE_PERMISSION', p);

        return p;
    }

    @Action({ rawError: true })
    async updatePermission(updatePermission: PermissionUpdateRequest) {
        const p = await permissionService.updatePermission(updatePermission);
        this.context.commit('UPDATE_PERMISSION', p);

        return p;
    }

    @Action({ rawError: true })
    async deletePermission(permission: Permission) {
        await permissionService.deletePermission(permission);
        this.context.commit('DELETE_PERMISSION', permission);
    }

    @Action({ rawError: true })
    async createRole(createRole: RoleCreateRequest) {
        const r = await roleService.createRole(createRole);
        this.context.commit('CREATE_ROLE', r);

        return r;
    }

    @Action({ rawError: true })
    async updateRole(updateRole: RoleUpdateRequest) {
        const r = await roleService.updateRole(updateRole);
        this.context.commit('UPDATE_ROLE', r);

        return r;
    }

    @Action({ rawError: true })
    async deleteRole(role: Role) {
        await roleService.deleteRole(role);
        this.context.commit('DELETE_ROLE', role);
    }
}

export default getModule(SecurityStore);
