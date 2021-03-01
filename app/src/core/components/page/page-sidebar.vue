<template>
    <b-sidebar
        class="has-h-100"
        type="is-light"
        :fullheight="true"
        :overlay="isMobile"
        :open.sync="isOpen"
        :position="position"
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
import appStore from '@/core/store/app-store';
import { Component, Vue, Prop } from 'vue-property-decorator';

@Component({ name: 'page-sidebar' })
export default class PageSidebar extends Vue {
    @Prop({ default: false })
    overlay!: boolean;

    get isOpen() {
        return appStore.sidebar;
    }

    set isOpen(v: boolean) {
        appStore.TOGGLE_SIDEBAR(v);
    }

    position: 'fixed' | 'static' = 'static';
    isMobile: boolean = false;

    created() {
        this.checkIfMobile();
    }

    mounted() {
        window.addEventListener('resize', this.checkIfMobile);
    }

    beforeDestroy() {
        window.removeEventListener('resize', this.checkIfMobile);
    }

    checkIfMobile() {
        this.isMobile = window.innerWidth <= 768; // Needs to match bulma
        this.position = !this.isMobile ? 'static' : 'fixed';

        if (!this.isMobile) {
            appStore.SHOW_SIDEBAR();
        }
    }
}
</script>
