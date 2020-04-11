<template>
    <div id="app">
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
import { getModule } from 'vuex-module-decorators';
import UserStore from '@/modules/app/store/user/user-store';
import LoadingSplash from '@/core/components/loading-splash.vue';

@Component({
    name: 'app',
    components: {
        LoadingSplash
    }
})
export default class App extends Vue {
    get isLoggingIn() {
        const userStore = getModule(UserStore, this.$store);
        return userStore.isLoading;
    }

    async created() {
        const userStore = getModule(UserStore, this.$store);
        await userStore.init();

        // Redirect logged in users to the calendar
        if (userStore.isAuthenticated) {
            this.$router.push({ name: 'calendar' });
        }
    }
}
</script>
