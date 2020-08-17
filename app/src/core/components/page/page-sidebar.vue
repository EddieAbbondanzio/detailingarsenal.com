<template>
    <b-sidebar
        class="has-h-100"
        type="is-light"
        :fullheight="true"
        :overlay="overlay"
        :open.sync="isOpen"
        :position="!overlay ? 'static' : 'fixed'"
    >
        <div class="has-padding-all-3">
            <slot></slot>
        </div>
    </b-sidebar>
</template>

<style lang="sass" scoped>
.sidebar
    width: 300px
    border-right: 1px solid $grey-lighter
</style>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';

@Component({})
export default class PageSidebar extends Vue {
    @Prop({ default: false })
    overlay!: boolean;

    isOpen: boolean = !this.overlay;

    open() {
        this.isOpen = true;
    }
}
</script>