<template>
    <page>
        <template v-slot:header>
            <page-header title="Create role" :description="`Create new role`">
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="Admin Panel" :to="{name: 'adminPanel'}" />
                        <breadcrumb name="Scheduling Panel" :to="{name: 'schedulingPanel'}" />
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
                    <b-table-column label="Permission" field="label" sortable>{{ props.row.label }}</b-table-column>
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
import { Permission, SpecificationError } from '@/api';
import { displayError, toast, displayLoading } from '@/core';
import accessControlStore from '../../store/access-control-store';

@Component({
    name: 'create-role'
})
export default class CreateRole extends Vue {
    get permissions() {
        return accessControlStore.permissions;
    }

    name = '';
    enabledPermissions: Permission[] = [];

    @displayLoading
    async created() {
        await accessControlStore.init();
    }

    @displayLoading
    async onSubmit() {
        const create = {
            name: this.name,
            permissionIds: this.enabledPermissions.map(p => p.id)
        };

        try {
            const role = await accessControlStore.createRole(create);

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