<template>
    <page>
        <template v-slot:header>
            <page-header title="Create role" :description="`Create new role`">
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="Admin Panel" :to="{name: 'adminPanel'}" />
                        <breadcrumb name="Roles" :to="{name: 'roles'}" />
                        <breadcrumb name="Create" :to="{name: 'createRole'}" active="true" />
                    </breadcrumb-trail>
                </template>
            </page-header>
        </template>

        <input-form @submit="onSubmit" submitText="Create">
            <input-text-field
                label="Name"
                rules="required|max:32"
                :required="true"
                v-model="name"
                placeholder="user"
            />

            <input-group-header text="Permissions" />
            <b-table :data="permissions" checkable :checked-rows.sync="enabledPermissions">
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
    name: 'create-role'
})
export default class CreateRole extends Vue {
    name = '';
    permissions: { id: string; permission: string; action: string; scope: string }[] = [];
    enabledPermissions: { id: string; permission: string; action: string; scope: string }[] = [];

    @displayLoading
    async created() {
        await adminStore.init();
        this.permissions = adminStore.permissions
            .map(p => ({
                enabled: false,
                id: p.id,
                permission: p.toString(),
                action: p.action,
                scope: p.scope
            }))
            .sort((a, b) => (a.scope > b.scope ? 1 : -1));
    }

    @displayLoading
    async onSubmit() {
        const create = {
            name: this.name,
            permissionIds: this.enabledPermissions.map(p => p.id)
        };

        try {
            const role = await adminStore.createRole(create);

            toast(`Created new role ${role.name}`);
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