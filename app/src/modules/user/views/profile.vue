<template>
    <page>
        <template v-slot:header>
            <page-header title="Profile" description="Personal information">
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="Account" :to="{ name: 'account' }" />
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
                <h5 class="is-size-4 title has-margin-bottom-2">Name</h5>
                <span class="is-size-6 subtitle">{{ user.name }}</span>
            </div>

            <div class="has-margin-bottom-3">
                <h5 class="is-size-4 title has-margin-bottom-2">Email</h5>
                <span class="is-size-6 subtitle">{{ user.email }}</span>
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
import userStore from '../store/user-store';
import { displayLoading } from '../../../core/utils/display-loading';

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
