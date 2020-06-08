<template>
    <page>
        <template v-slot:header>
            <page-header title="Edit role" :description="`Edit new role`">
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="Admin Panel" :to="{name: 'adminPanel'}" />
                        <breadcrumb name="Roles" :to="{name: 'roles'}" />
                        <breadcrumb name="Edit" :to="{name: 'editRole'}" active="true" />
                    </breadcrumb-trail>
                </template>
            </page-header>
        </template>

        <input-form @submit="onSubmit" submitText="Save changes">
            <input-text-field
                label="Name"
                rules="required|max:32"
                :required="true"
                v-model="name"
                placeholder="user"
            />

            <input-group-header text="Permissions" />
            <b-table
                :data="permissions"
                checkable
                :checked-rows.sync="enabledPermissions"
                :custom-is-checked="(a, b) => { return a.id === b.id }"
            >
                <template slot-scope="props">
                    <b-table-column
                        label="Permission"
                        field="permission"
                        sortable
                    >{{ props.row.permission }}</b-table-column>
                    <b-table-column label="Action" field="action" sortable>{{ props.row.action }}</b-table-column>
                    <b-table-column label="Scope" field="scope" sortable>{{ props.row.scope }}</b-table-column>
                </template>

                <template slot="empty">
                    <div class="is-flex is-justify-content-center">There's nothing here!</div>
                </template>
            </b-table>
        </input-form>
    </page>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import adminStore from '../../store/admin-store';
import { Permission } from '../../api/entities/permission';
import { displayLoading } from '../../../../core/utils/display-loading';
import { SpecificationError, displayError, toast } from '@/core';

@Component({
    name: 'edit-role'
})
export default class EditRole extends Vue {
    name = '';
    permissions: { id: string; permission: string; action: string; scope: string }[] = [];
    enabledPermissions: { id: string; permission: string; action: string; scope: string }[] = [];

    @displayLoading
    async created() {
        await adminStore.init();

        const role = adminStore.roles.find(r => r.id == this.$route.params.id);

        this.name = role!.name;
        this.permissions = adminStore.permissions
            .map(p => ({
                enabled: role!.permissionIds.some(id => id == p.id),
                id: p.id,
                permission: p.toString(),
                action: p.action,
                scope: p.scope
            }))
            .sort((a, b) => (a.scope > b.scope ? 1 : -1));
        this.enabledPermissions = role!.permissionIds
            .map(id => adminStore.permissions.find(p => p.id == id)!)
            .map(p => ({
                id: p.id,
                permission: p.toString(),
                action: p.action,
                scope: p.scope
            }));
    }

    @displayLoading
    async onSubmit() {
        const Edit = {
            id: this.$route.params.id,
            name: this.name,
            permissionIds: this.enabledPermissions.map(p => p.id)
        };

        try {
            const role = await adminStore.updateRole(Edit);

            toast(`Updated role ${role.name}`);
            this.$router.push({ name: 'roles' });
        } catch (err) {
            if (err instanceof SpecificationError) {
                displayError(err);
            } else {
                throw err;
            }
        }
    }
}
</script>