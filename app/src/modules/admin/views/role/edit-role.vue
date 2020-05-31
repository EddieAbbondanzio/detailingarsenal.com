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
    name: 'edit-role'
})
export default class EditRole extends Vue {
    name = '';
    permissions: { enabled: boolean; permission: Permission }[] = [];

    @displayLoading
    async created() {
        await adminStore.init();

        const role = adminStore.roles.find(r => r.id == this.$route.params.id);

        this.name = role!.name;
        this.permissions = adminStore.permissions.map(p => ({
            enabled: role!.permissionIds.some(id => id == p.id),
            permission: p
        }));
    }

    @displayLoading
    async onSubmit() {
        const Edit = {
            id: this.$route.params.id,
            name: this.name,
            permissionIds: this.permissions.filter(p => p.enabled).map(p => p.permission.id)
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