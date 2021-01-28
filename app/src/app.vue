<template>
    <div id="app" class="is-flex is-flex-column is-flex-grow-1">
        <router-view />
    </div>
</template>

<style lang="sass">
#app
    height: calc(100vh - 56px)

.app-content
    // overflow-y: hidden
    display: flex
    flex-direction: column
    flex-grow: 1

+touch
    .app-content
        margin-bottom: 50px

    // nav footer
    .app-footer
        clear: both
        position: fixed
        height: 50px
        left: 0
        right: 0
        bottom: 0

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
