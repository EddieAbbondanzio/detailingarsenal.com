<template>
    <page>
        <template v-slot:header>
            <page-header title="Create permission" :description="`Create new permission`">
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="Admin panel" :to="{ name: 'adminPanel' }" />
                        <breadcrumb name="Users panel" :to="{ name: 'usersPanel' }" />
                        <breadcrumb name="Permissions" :to="{ name: 'permissions' }" />
                        <breadcrumb name="Create" :to="{ name: 'createPermission' }" active="true" />
                    </breadcrumb-trail>
                </template>
            </page-header>
        </template>

        <input-form @submit="onSubmit" submitText="Create">
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
import appStore from '@/core/store/app-store';
import { displayLoading } from '@/core/utils/display-loading';
import { SpecificationError } from '@/api';
import securityStore from '../../store/security-store';

@Component({
    name: 'create-permission',
})
export default class CreatePermission extends Vue {
    action: string = '';
    scope: string = '';

    @displayLoading
    async onSubmit() {
        const create = { action: this.action, scope: this.scope };

        try {
            await securityStore.createPermission(create);

            toast(`Created new permission`);
            this.$router.push({ name: 'permissions' });
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