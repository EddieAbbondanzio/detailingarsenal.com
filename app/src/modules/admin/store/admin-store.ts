import { Module, VuexModule, Mutation, Action, getModule } from 'vuex-module-decorators';
import { InitableModule } from '@/core/store/initable-module';
import { Permission } from '@/modules/admin/api/entities/permission';
import { api } from '@/core/api/api';
import { CreatePermission } from '@/modules/admin/api/data-transfer-objects/create-permission';
import { UpdatePermission } from '@/modules/admin/api/data-transfer-objects/update-permission';
import store from '@/core/store/index';
import { Role } from '@/modules/admin/api/entities/role';
import { CreateRole } from '@/modules/admin/api/data-transfer-objects/create-role';
import { UpdateRole } from '@/modules/admin/api/data-transfer-objects/update-role';
import { SubscriptionPlan } from '@/modules/admin/api/entities/subscription-plan';
import { UpdateSubscriptionPlan } from '@/modules/admin/api/data-transfer-objects/update-subscription-plan';

/**
 * Store for the schedule view.
 */
@Module({ namespaced: true, name: 'admin', dynamic: true, store })
class AdminStore extends InitableModule {
    permissions: Permission[] = [];
    roles: Role[] = [];
    subscriptionPlans: SubscriptionPlan[] = [];

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

    @Mutation
    SET_SUBSCRIPTION_PLANS(plans: SubscriptionPlan[]) {
        this.subscriptionPlans = plans;
    }

    @Mutation
    UPDATE_SUBSCRIPTION_PLAN(plan: SubscriptionPlan) {
        this.subscriptionPlans = [...this.subscriptionPlans.filter(p => p.id != plan.id), plan];
    }

    @Action({ rawError: true })
    async _init() {
        const [perms, roles, plans] = await Promise.all([
            api.security.permission.getPermissions(),
            api.security.role.getRoles(),
            api.billing.subscriptionPlan.getPlans()
        ]);

        this.context.commit('SET_PERMISSIONS', perms);
        this.context.commit('SET_ROLES', roles);
        this.context.commit('SET_SUBSCRIPTION_PLANS', plans);
    }

    @Action({ rawError: true })
    async createPermission(createPermission: CreatePermission) {
        const p = await api.security.permission.createPermission(createPermission);
        this.context.commit('CREATE_PERMISSION', p);

        return p;
    }

    @Action({ rawError: true })
    async updatePermission(updatePermission: UpdatePermission) {
        const p = await api.security.permission.updatePermission(updatePermission);
        this.context.commit('UPDATE_PERMISSION', p);

        return p;
    }

    @Action({ rawError: true })
    async deletePermission(permission: Permission) {
        await api.security.permission.deletePermission(permission);
        this.context.commit('DELETE_PERMISSION', permission);
    }

    @Action({ rawError: true })
    async createRole(createRole: CreateRole) {
        const r = await api.security.role.createRole(createRole);
        this.context.commit('CREATE_ROLE', r);

        return r;
    }

    @Action({ rawError: true })
    async updateRole(updateRole: UpdateRole) {
        const r = await api.security.role.updateRole(updateRole);
        this.context.commit('UPDATE_ROLE', r);

        return r;
    }

    @Action({ rawError: true })
    async deleteRole(role: Role) {
        await api.security.role.deleteRole(role);
        this.context.commit('DELETE_ROLE', role);
    }

    @Action({ rawError: true })
    async refreshSubscriptionPlans() {
        const plans = await api.billing.subscriptionPlan.refreshPlans();
        this.context.commit('SET_SUBSCRIPTION_PLANS', plans);
        return plans;
    }

    @Action({ rawError: true })
    async updateSubscriptionPlan(updateSubscriptionPlan: UpdateSubscriptionPlan) {
        const plan = await api.billing.subscriptionPlan.updatePlan(updateSubscriptionPlan);
        this.context.commit('UPDATE_SUBSCRIPTION_PLAN', plan);
        return plan;
    }
}

export default getModule(AdminStore);
