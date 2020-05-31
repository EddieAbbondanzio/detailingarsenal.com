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

            <div>
                <p class="is-size-6 has-text-weight-bold">Permissions</p>
                <hr class="has-margin-top-0 has-padding-y-0 has-margin-bottom-3" />
            </div>

            <input-checkbox
                v-for="p in permissions"
                :key="p.id"
                :label="p.permission.toString()"
                v-model="p.enabled"
            />
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
    permissions: { enabled: boolean; permission: Permission }[] = [];

    @displayLoading
    async created() {
        await adminStore.init();
        this.permissions = adminStore.permissions.map(p => ({ enabled: false, permission: p }));
    }

    @displayLoading
    async onSubmit() {
        const create = {
            name: this.name,
            permissionIds: this.permissions.filter(p => p.enabled).map(p => p.permission.id)
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