<template>
    <div
        class="loading-splash is-flex is-flex-column is-flex-grow-1 has-background-primary is-align-items-center is-justify-content-center"
    >
        <div class="is-flex is-flex-column is-align-items-center">
            <h1
                class="is-size-3 has-text-white has-font-family-pacifico has-margin-bottom-3"
            >Detailing Arsenal</h1>
            <p class="has-text-white">Thanks for using Detailing Arsenal! See you soon.</p>

            <div class="is-flex is-flex-row has-margin-top-3">
                <b-button
                    class="has-margin-right-1"
                    type="is-white"
                    outlined
                    @click="onLogIn"
                >Log in</b-button>
                <b-button
                    class="has-margin-left-1"
                    type="is-white"
                    outlined
                    @click="onLogIn"
                >Sign up</b-button>
            </div>
        </div>
    </div>
</template>

<style lang="sass">
.loading-splash
    .loading-background
        background-color: transparent!important
</style>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import userStore from '../../modules/user/store/user-store';

@Component({
    name: 'goodbye'
})
export default class Goodbye extends Vue {
    async onLogIn() {
        userStore.login();
    }
}
</script>