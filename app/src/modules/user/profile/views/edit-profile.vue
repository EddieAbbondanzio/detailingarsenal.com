<template>
    <page>
        <template v-slot:header>
            <page-header title="Profile" description="Edit personal information">
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="User" :to="{ name: 'user' }" />
                        <breadcrumb name="Profile" :to="{ name: 'profile' }" />
                        <breadcrumb name="Edit" :to="{ name: 'editProfile' }" :active="true" />
                    </breadcrumb-trail>
                </template>
            </page-header>
        </template>

        <input-form @submit="onSubmit">
            <input-text-field label="Name" rules="max:64" v-model="name" placeholder="John Smith" />
            <input-text-field
                label="Email"
                rules="email|max:320"
                v-model="email"
                placeholder="john.smith@fake.com"
                :disabled="true"
            />
        </input-form>
    </page>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { toast, displayLoading } from '@/core';
import userStore from '../../core/store/user-store';

@Component
export default class EditProfile extends Vue {
    get user() {
        return userStore.user;
    }

    public name: string = '';
    public email: string = '';

    @displayLoading
    public async created() {
        await userStore.init();
        this.email = JSON.parse(JSON.stringify(userStore.user.email));

        if (userStore.user.name != null) {
            this.name = JSON.parse(JSON.stringify(userStore.user.name));
        }
    }

    @displayLoading
    public async onSubmit() {
        await userStore.updateUser({ name: this.name });

        toast(`Edited user profile`);
        this.$router.push({ name: 'profile' });
    }
}
</script>
