<template>
    <page>
        <template v-slot:header>
            <page-header title="Edit permission" :description="`Edit an existing permission`">
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="Admin panel" :to="{ name: 'adminPanel' }" />
                        <breadcrumb name="Users panel" :to="{ name: 'usersPanel' }" />
                        <breadcrumb name="Permissions" :to="{ name: 'permissions' }" />
                        <breadcrumb name="Edit" :to="{ name: 'editPermission', params: $route.params }" active="true" />
                    </breadcrumb-trail>
                </template>
            </page-header>
        </template>

        <input-form @submit="onSubmit" submitText="Save changes">
            <input-text-field
                label="Action"
                rules="required|max:32"
                :required="true"
                v-model="action"
                placeholder="read"
            />

            <input-text-field
                label="Scope"
                rules="required|max:32"
                v-model="scope"
                placeholder="users"
                :required="true"
            />
        </input-form>
    </page>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { toast, displayError, displayLoading } from '@/core';
import { Permission, SpecificationError } from '@/api';
import securityStore from '../../store/security-store';

@Component({
    name: 'edit-permission',
})
export default class EditPermission extends Vue {
    action: string = '';
    scope: string = '';

    @displayLoading
    async created() {
        const id = this.$route.params.id;
        await securityStore.init();

        const perm = securityStore.permissions.find((p) => p.id == id);

        if (perm == null) {
            throw new Error(`Permission with id ${id} does not exist.`);
        }

        this.action = perm.action;
        this.scope = perm.scope;
    }

    @displayLoading
    async onSubmit() {
        const edit = { id: this.$route.params.id, action: this.action, scope: this.scope };

        try {
            await securityStore.updatePermission(edit);

            toast(`Updated permission`);
            this.$router.push({ name: 'permissions' });
        } catch (err) {
            displayError(err);
        }
    }
}
</script>