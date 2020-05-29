import { Module, VuexModule, Mutation, Action, getModule } from 'vuex-module-decorators';
import { InitableModule } from '@/core/store/initable-module';
import { Permission } from '@/modules/admin/api/entities/permission';
import { api } from '@/core/api/api';
import { CreatePermission } from '@/modules/admin/api/data-transfer-objects/create-permission';
import { UpdatePermission } from '@/modules/admin/api/data-transfer-objects/update-permission';
import store from '@/core/store/index';

/**
 * Store for the schedule view.
 */
@Module({ namespaced: true, name: 'admin', dynamic: true, store })
class AdminStore extends InitableModule {
    permissions: Permission[] = [];

    @Mutation
    SET_PERMISSIONS(permissions: Permission[]) {
        this.permissions = permissions;
    }

    @Mutation
    CREATE_PERMISSION(permission: Permission) {
        this.permissions.push(permission);
    }

    @Mutation
    UPDATE_PERMISSION(permission: Permission) {
        this.permissions = [...this.permissions.filter(p => p.id != permission.id), permission];
    }

    @Mutation
    DELETE_PERMISSION(id: string) {
        const index = this.permissions.findIndex(p => p.id == id);

        if (index != -1) {
            this.permissions.splice(index, 1);
        }
    }

    @Action({ rawError: true })
    async _init() {
        const perms = await api.authorization.permission.getPermissions();
        this.context.commit('SET_PERMISSIONS', perms);
    }

    @Action({ rawError: true })
    async createPermission(createPermission: CreatePermission) {
        const p = await api.authorization.permission.createPermission(createPermission);
        this.context.commit('CREATE_PERMISSION', p);
    }

    @Action({ rawError: true })
    async updatePermission(updatePermission: UpdatePermission) {
        const p = await api.authorization.permission.updatePermission(updatePermission);
        this.context.commit('UPDATE_PERMISSION', p);
    }

    @Action({ rawError: true })
    async deletePermission(permission: Permission) {
        await api.authorization.permission.deletePermission(permission.id);
        this.context.commit('DELETE_PERMISSION', permission.id);
    }
}

export default getModule(AdminStore);
