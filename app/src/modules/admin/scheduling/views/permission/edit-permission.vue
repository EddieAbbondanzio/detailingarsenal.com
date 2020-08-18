<template>
    <page :loading="loading">
        <template v-slot:header>
            <page-header title="Edit permission" :description="`Edit an existing permission`">
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="Admin Panel" :to="{name: 'adminPanel'}" />
                        <breadcrumb name="Scheduling Panel" :to="{name: 'schedulingPanel'}" />
                        <breadcrumb name="Permissions" :to="{name: 'permissions'}" />
                        <breadcrumb
                            name="Edit"
                            :to="{name: 'editPermission', params: $route.params}"
                            active="true"
                        />
                    </breadcrumb-trail>
                </template>
            </page-header>
        </template>

        <input-form @submit="onSubmit" :loading="loading" submitText="Save changes">
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
import { toast, displayError } from '@/core';
import { Permission, SpecificationError } from '@/api';
import accessControlStore from '../../store/access-control-store';

@Component({
    name: 'edit-permission'
})
export default class EditPermission extends Vue {
    loading = false;
    action: string = '';
    scope: string = '';

    async created() {
        const id = this.$route.params.id;
        await accessControlStore.init();

        const perm = accessControlStore.permissions.find(p => p.id == id);

        if (perm == null) {
            throw new Error(`Permission with id ${id} does not exist.`);
        }

        this.action = perm.action;
        this.scope = perm.scope;
        this.loading = false;
    }

    async onSubmit() {
        this.loading = true;
        const edit = { id: this.$route.params.id, action: this.action, scope: this.scope };

        try {
            await accessControlStore.updatePermission(edit);

            toast(`Created new permission`);
            this.$router.push({ name: 'permissions' });
        } catch (err) {
            if (err instanceof SpecificationError) {
                displayError(err);
            } else {
                throw err;
            }
        } finally {
            this.loading = false;
        }
    }
}
</script>