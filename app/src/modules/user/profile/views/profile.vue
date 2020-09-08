<template>
    <page>
        <template v-slot:header>
            <page-header title="Profile " description="Personal information">
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="User" :to="{ name: 'user' }" />
                        <breadcrumb name="Profile" :to="{ name: 'profile'}" active="true" />
                    </breadcrumb-trail>
                </template>

                <template v-slot:action>
                    <edit-button :to="{ name: 'editProfile' }" />
                </template>
            </page-header>
        </template>

        <div class="box is-shadowless" v-if="user != null">
            <div class="has-margin-bottom-3">
                <h5 class="is-size-4 title has-margin-bottom-2">Username</h5>
                <span class="is-size-6 subtitle">{{ user.username }}</span>
            </div>

            <div class="has-margin-bottom-3">
                <h5 class="is-size-4 title has-margin-bottom-2">Email</h5>
                <span class="is-size-6 subtitle">{{ user.email }}</span>
            </div>

            <div class="has-margin-bottom-3">
                <h5 class="is-size-4 title has-margin-bottom-2">
                    Name
                    <small class="has-text-grey-light is-size-5">(Hidden)</small>
                </h5>
                <span class="is-size-6 subtitle">{{ user.name }}</span>
            </div>

            <div class="has-margin-bottom-3">
                <h5 class="is-size-4 title has-margin-bottom-2">Joined Date</h5>
                <span class="is-size-6 subtitle">{{ user.joinedDate | date }}</span>
            </div>
        </div>
    </page>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { getModule } from 'vuex-module-decorators';
import { displayLoading } from '@/core';
import userStore from '@/modules/user/core/store/user-store';

@Component
export default class Profile extends Vue {
    get user() {
        return userStore.user;
    }

    @displayLoading
    async created() {
        await userStore.init();
    }
}
</script>
