<template>
    <div v-if="isAuthenticated">
        <b-dropdown aria-role="list" position="is-bottom-left">
            <button class="button is-primary" slot="trigger">
                <b-icon icon="account" size="is-medium" type="is-light" class="has-padding-all-3"></b-icon>
            </button>

            <b-dropdown-item aria-role="listitem" custom>
                <router-link
                    :to="{name: 'user' }"
                    class="is-flex is-flex-column is-align-items-center has-text-dark has-padding-y-3"
                >
                    <b-icon
                        icon="account"
                        size="is-large"
                        type="is-dark"
                        class="has-padding-bottom-3"
                    ></b-icon>
                    {{ username }}
                </router-link>
            </b-dropdown-item>
            <b-dropdown-item aria-role="listitem" has-link>
                <router-link :to="{name: 'profile'}" class="is-flex is-flex-row">
                    <b-icon icon="account" type="is-dark" class="has-padding-right-3" />Profile
                </router-link>
            </b-dropdown-item>

            <b-dropdown-item aria-role="listitem" has-link>
                <router-link :to="{name: 'subscription'}" class="is-flex is-flex-row">
                    <b-icon icon="currency-usd" type="is-dark" class="has-padding-right-3" />Subscription
                </router-link>
            </b-dropdown-item>

            <b-dropdown-item
                aria-role="listitem"
                @click="onLogoutClick"
                class="is-flex is-flex-row"
            >
                <b-icon icon="logout" type="is-danger" class="has-padding-right-3" />Log out
            </b-dropdown-item>
        </b-dropdown>
    </div>
    <div class="has-margin-right-2-mobile" v-else>
        <b-button type="is-success" class="has-margin-right-2" @click="signUp()">Sign up</b-button>
        <b-button type="is-info" @click="login()">Login</b-button>
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import userStore from '../store/user-store';

@Component({
    name: 'user-widget'
})
export default class UserWidget extends Vue {
    get isAuthenticated() {
        return userStore.isAuthenticated;
    }

    get username() {
        if (userStore.user == null) {
            return '';
        }

        return userStore.user.username;
    }

    async login() {
        userStore.login(this.$route.path);
    }

    async signUp() {
        userStore.signUp(this.$route.path);
    }

    async onLogoutClick() {
        await userStore.logout();
    }
}
</script>