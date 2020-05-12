<template>
    <div>
        <public-navbar />
        <router-view />
    </div>
</template>

<style lang="sass">
body
    overflow-y: scroll!important
</style>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import PublicNavbar from '@/modules/public/components/public-navbar.vue';

@Component({
    name: 'landing',
    components: {
        PublicNavbar
    }
})
export default class Public extends Vue {}
</script>