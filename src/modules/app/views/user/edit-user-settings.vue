<template>
    <page
        title="Edit User Settings"
        description="Edit name, email, and more"
        :breadcrumbs="[
                { name: 'User Settings', to: { name: 'userSettings' } },
                { name: 'Edit', to: { name: 'editUserSettings' } },
            ]"
        actionText="Save changes"
        :loading="loading"
        @input="onSubmit"
    >
        <template v-slot:header>
            <page-header title="Edit user settings" description="Edit name, and email">
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="User settings" :to="{name: 'userSettings'}" />
                        <breadcrumb name="Edit" :to="{name: 'editUserSettings'}" :active="true" />
                    </breadcrumb-trail>
                </template>
            </page-header>
        </template>

        <input-form @submit="onSubmit" :loading="loading">
            <input-text-field label="Name" rules="max:64" v-model="name" placeholder="John Smith" />
            <input-text-field
                label="Email"
                rules="email|max:320"
                v-model="email"
                placeholder="john.smith@fake.com"
            />
        </input-form>
    </page>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { getModule } from 'vuex-module-decorators';
import UserStore from '../../store/user/user-store';
import { toast } from '@/core';

@Component({
    name: 'edit-user-settings'
})
export default class EditUserSettings extends Vue {
    get user() {
        const userStore = getModule(UserStore, this.$store);
        return userStore.user;
    }

    public name: string = '';
    public email: string = '';

    public loading: boolean = true;

    public async created() {
        const userStore = getModule(UserStore, this.$store);
        await userStore.init;

        this.email = JSON.parse(JSON.stringify(userStore.user.email));

        if (userStore.user.name != null) {
            this.name = JSON.parse(JSON.stringify(userStore.user.name));
        }

        this.loading = false;
    }

    public async onSubmit() {
        this.loading = true;
        const userStore = getModule(UserStore, this.$store);

        await userStore.updateUser({ name: this.name });

        toast(`Edited user settings`);
        this.$router.push({ name: 'userSettings' });
        this.loading = false;
    }
}
</script>