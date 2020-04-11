<template>
    <b-navbar type="is-primary" :fixed-top="true">
        <template slot="brand">
            <b-navbar-item exact tag="a" @click="$router.push({name: 'landing'})">
                <h1
                    class="is-size-5-mobile is-size-4-tablet has-font-family-pacifico"
                >Detailing Arsenal</h1>
            </b-navbar-item>
        </template>

        <template slot="start">
            <b-navbar-item exact tag="router-link" :to="{ name: 'pricing' }">Pricing</b-navbar-item>
            <b-navbar-item exact tag="router-link" :to="{ name: 'aboutUs' }">About Us</b-navbar-item>
            <b-navbar-item exact tag="router-link" :to="{ name: 'contact' }">Contact</b-navbar-item>
        </template>

        <template slot="end">
            <b-navbar-item tag="div">
                <div class="buttons is-relative">
                    <b-button
                        class="has-text-weight-bold has-text-white is-success has-text-decoration-none"
                        type="is-text"
                        @click="onLogin"
                    >Sign up</b-button>

                    <b-button
                        class="has-text-weight-bold has-text-white is-info has-text-decoration-none"
                        type="is-text"
                        @click="onLogin"
                    >Log in</b-button>
                </div>
            </b-navbar-item>
        </template>
    </b-navbar>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { getModule } from 'vuex-module-decorators';
import UserStore from '../../app/store/user/user-store';

@Component({
    name: 'public-navbar'
})
export default class PublicNavbar extends Vue {
    onLogin() {
        const userStore = getModule(UserStore, this.$store);
        return userStore.login();
    }
}
</script>