<template>
    <div id="app" class="is-flex is-flex-column is-flex-grow-1" style="height: 100vh!important;">
        <router-view v-if="!isLoggingIn" />
        <loading-splash v-else />
    </div>
</template>

<style lang="sass">
// Shift toasts above footer nav bar on mobile
.notices.is-bottom
    padding-bottom: 64px
</style>

<script lang="ts">
import { Component, Vue, Prop, Ref } from 'vue-property-decorator';
import LoadingSplash from '@/core/components/loading-splash.vue';
import userStore from './modules/user/store/user-store';

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
        await userStore.init();
    }
}
</script>
