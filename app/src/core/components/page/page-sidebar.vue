<template>
    <div class="has-padding-all-3 sidebar has-background-light is-flex is-flex-column has-h-100">
        <slot></slot>
    </div>
</template>

<style lang="sass" scoped>
.sidebar
    width: 300px
    border-right: 1px solid $grey-lighter
</style>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';

@Component({})
export default class PageSidebar extends Vue {}
</script>