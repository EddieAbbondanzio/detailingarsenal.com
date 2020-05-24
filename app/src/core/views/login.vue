<template>
    <div>Redirecting to login page. Please wait...</div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import userStore from '../../modules/user/store/user-store';

@Component({
    name: 'login'
})
export default class Login extends Vue {
    async created() {
        await userStore.init();

        if (!userStore.isAuthenticated) {
            await userStore.login();
        }
    }
}
</script>