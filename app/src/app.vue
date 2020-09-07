<template>
    <div id="app" class="is-flex is-flex-column is-flex-grow-1" style="height: 100vh!important;">
        <router-view v-if="!isLoggingIn" />
        <loading-splash v-else />
    </div>
</template>

<style lang="sass">
.app-content
    overflow-y: hidden
    display: flex
    flex-direction: column
    flex-grow: 1

+touch
    .app-content
        margin-bottom: 50px

// Shift toasts above footer nav bar on mobile
.notices.is-bottom
    padding-bottom: 64px
</style>

<script lang="ts">
import { Component, Vue, Prop, Ref } from 'vue-property-decorator';
import LoadingSplash from '@/core/components/loading-splash.vue';
import userStore from './modules/user/core/store/user-store';
import { loadStripeJs } from '@/plugins/stripe';

@Component({
    name: 'app',
    components: {
        LoadingSplash
    }
})
export default class App extends Vue {
    get isLoggingIn() {
        return userStore.isLoading;
    }

    async created() {
        await loadStripeJs();
        await userStore.init();
    }
}
</script>
