<template>
    <page>
        <template v-slot:header>
            <page-header :title="role != null ? role.name : ``">
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="Admin" :to="{name: 'adminPanel'}" />
                        <breadcrumb name="Roles" :to="{name: 'roles'}" />
                        <breadcrumb
                            :name="role != null ? role.name : ''"
                            :to="{name: 'role', params: $route.params}"
                            :active="true"
                        />
                    </breadcrumb-trail>
                </template>

                <template v-slot:action>
                    <edit-button :to="{name: 'editRole', params: { id: $route.params.id }}" />
                </template>
            </page-header>
        </template>

        <div class="box is-shadowless" v-if="role != null">
            <div class="has-margin-bottom-4">
                <h5 class="is-size-4 title has-margin-bottom-2">{{ role.name }}</h5>
            </div>

            <input-group-header text="Permissions" />

            <input-checkbox
                v-for="p in permissions"
                :key="p.id"
                :label="p.permission.toString()"
                v-model="p.enabled"
                :disabled="true"
            />
        </div>
    </page>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { Role } from '../../api/entities/role';
import adminStore from '../../store/admin-store';
import { displayLoading } from '../../../../core/utils/display-loading';
import { Permission } from '../../api/entities/permission';
import appStore from '../../../../core/store/app-store';

@Component({
    name: 'role'
})
export default class RoleView extends Vue {
    role: Role = null!;
    permissions: { enabled: boolean; permission: Permission }[] = [];

    @displayLoading
    async created() {
        await adminStore.init();

        this.role = adminStore.roles.find(r => r.id == this.$route.params.id)!;

        this.permissions = adminStore.permissions.map(p => ({
            enabled: this.role!.permissionIds.some(id => id == p.id),
            permission: p
        }));
    }
}
</script>