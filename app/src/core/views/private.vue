<template>
    <div class="is-flex is-flex-column is-flex-grow-1 has-overflow-y-hidden" v-if="!isLoggingIn">
        <private-navbar />

        <div class="app-content has-overflow-y-hidden is-flex is-flex-column is-flex-grow-1">
            <router-view />
        </div>

        <private-footer />
    </div>
    <loading-splash v-else />
</template>

<style lang="sass" scoped>
+touch
    .app-content
        margin-bottom: 50px
</style>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import PrivateNavbar from '@/core/components/private-navbar.vue';
import PrivateFooter from '@/core/components/private-footer.vue';
import { getModule } from 'vuex-module-decorators';
import LoadingSplash from '@/core/components/loading-splash.vue';
import userStore from '@/modules/scheduling/user/store/user-store';
import { loadStripeJs } from '@/plugins/stripe';

/**
 * Parent page for all private views
 */
@Component({
    name: 'private',
    components: {
        PrivateNavbar,
        PrivateFooter,
        LoadingSplash
    }
})
export default class Private extends Vue {
    get isLoggingIn() {
        return userStore.isLoading;
    }

    async created() {
        await loadStripeJs();
        await userStore.init();
    }
}
</script>
