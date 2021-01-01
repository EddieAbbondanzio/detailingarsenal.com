<template>
    <page>
        <template v-slot:header>
            <page-header :title="`${verb} permission`" :description="`${verb} permission`">
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="Admin panel" :to="{ name: 'adminPanel' }" />
                        <breadcrumb name="Users panel" :to="{ name: 'usersPanel' }" />
                        <breadcrumb name="Permissions" :to="{ name: 'permissions' }" />
                        <breadcrumb :name="verb" :to="$route" :active="true" />
                    </breadcrumb-trail>
                </template>
            </page-header>
        </template>

        <input-form @submit="onSubmit" :submitText="verb">
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
import InputViewMixin from '@/core/mixins/input-view-mixin';

@Component({
    name: 'create-permission',
})
export default class CreateOrUpdatePermission extends InputViewMixin {
    action: string = '';
    scope: string = '';

    @displayLoading
    async created() {
        if (this.mode == 'update') {
            await securityStore.init();

            const p = await securityStore.permissions.find((p) => p.id == this.id);

            if (p == null) {
                this.$router.go(-1);
                return;
            }

            this.action = p.action;
            this.scope = p.scope;
        }
    }

    @displayLoading
    async onSubmit() {
        try {
            switch (this.mode) {
                case 'create':
                    await securityStore.createPermission({ action: this.action, scope: this.scope });
                    break;
                case 'update':
                    await securityStore.updatePermission({ id: this.id, action: this.action, scope: this.scope });
                    break;
            }

            toast(`${this.verb} permission`);
            this.$router.push({ name: 'permissions' });
        } catch (err) {
            displayError(err);
        }
    }
}
</script>