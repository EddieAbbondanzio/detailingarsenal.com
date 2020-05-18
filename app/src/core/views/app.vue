<template>
    <div class="is-flex is-flex-column is-flex-grow-1 has-overflow-y-hidden">
        <app-navbar />

        <div class="app-content has-overflow-y-hidden is-flex is-flex-column is-flex-grow-1">
            <router-view />
        </div>

        <app-footer />
    </div>
</template>

<style lang="sass" scoped>
+touch
    .app-content
        margin-bottom: 50px
</style>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import AppNavbar from '@/core/components/app-navbar.vue';
import AppFooter from '@/core/components/app-footer.vue';
import { getModule } from 'vuex-module-decorators';

/**
 * Parent page for all private views
 */
@Component({
    name: 'private',
    components: {
        AppNavbar,
        AppFooter
    }
})
export default class App extends Vue {}
</script>
