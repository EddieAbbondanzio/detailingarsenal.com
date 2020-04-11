<template>
    <div>
        <b-navbar class="app-navbar" type="is-primary" :mobile-burger="true" :fixed-top="true">
            <template slot="brand">
                <b-navbar-item
                    class="is-flex is-flex-row is-align-items-center"
                    exact
                    tag="a"
                    @click="$router.push({name: 'calendar'})"
                >
                    <h1
                        class="is-size-5-mobile is-size-4-tablet has-font-family-pacifico"
                    >Detailing Arsenal</h1>
                </b-navbar-item>
            </template>

            <!-- Only shows on desktop and bigger -->
            <template slot="start">
                <b-navbar-item
                    class="is-flex-tablet is-hidden-mobile"
                    exact
                    tag="router-link"
                    :to="{ name: 'calendar' }"
                >Calendar</b-navbar-item>
                <b-navbar-item
                    class="is-flex-tablet is-hidden-mobile"
                    exact
                    tag="router-link"
                    :to="{ name: 'clients' }"
                >Clients</b-navbar-item>
                <b-navbar-item
                    class="is-flex-tablet is-hidden-mobile"
                    exact
                    tag="router-link"
                    :to="{ name: 'settings' }"
                >Settings</b-navbar-item>
            </template>

            <template slot="end">
                <b-navbar-item tag="div">
                    <user-widget />
                </b-navbar-item>
            </template>

            <!-- Hack.  -->
            <template slot="burger">
                <user-widget
                    class="is-hidden-desktop is-align-self-center"
                    style="margin-left: auto;"
                />
            </template>
        </b-navbar>
    </div>
</template>

<style lang="sass" scoped>
// friggen hack. Needed to show user-widget on mobile too
.app-navbar
    .navbar-brand
        .navbar-item
            background: $primary!important

    .navbar-menu
        display: flex!important
        background: $primary!important
        padding: 0px!important
</style>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { getModule } from 'vuex-module-decorators';
import UserStore from '../store/user/user-store';
import UserWidget from '@/modules/app/components/user/user-widget.vue';

@Component({
    name: 'app-navbar',
    components: {
        UserWidget
    }
})
export default class AppNavbar extends Vue {
    get isLoading() {
        const userStore = getModule(UserStore, this.$store);
        return userStore.isLoading;
    }

    get isAuthenticated() {
        const userStore = getModule(UserStore, this.$store);
        return userStore.isAuthenticated;
    }

    public async onLoginClick() {
        const userStore = getModule(UserStore, this.$store);
        await userStore.login();
    }
}
</script>
