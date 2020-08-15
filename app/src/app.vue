<template>
    <div id="app" class="is-flex is-flex-column is-flex-grow-1" style="height: 100vh!important;">
        <router-view />
    </div>
</template>

<style lang="sass">
// Shift toasts above footer nav bar on mobile
.notices.is-bottom
    padding-bottom: 64px
</style>

<script lang="ts">
import { Component, Vue, Prop, Ref } from 'vue-property-decorator';

@Component({
    name: 'app'
})
export default class App extends Vue {}
</script>
