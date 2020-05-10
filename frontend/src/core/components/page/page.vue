<template>
    <div id="page" :class="cssClass">
        <!-- No sidebar -->
        <div
            class="is-flex is-flex-column is-flex-grow-1 has-overflow-y-hidden"
            v-if="!$slots['sidebar']"
        >
            <b-progress
                class="is-absolute page-loading-bar has-margin-bottom-0"
                size="is-medium"
                type="is-warning"
                v-if="loading"
            />

            <!-- Page header -->
            <slot name="header"></slot>

            <!-- Page body -->
            <div
                class="is-flex is-flex-row is-flex-grow-1 has-overflow-y-scroll is-justify-content-center"
            >
                <div class="container has-margin-all-3">
                    <slot></slot>
                </div>

                <slot name="overlays"></slot>
            </div>
        </div>

        <!-- Sidebar View -->
        <div class="is-flex is-flex-row is-flex-grow-1 has-overflow-y-hidden" v-else>
            <div class="is-hidden-touch">
                <slot name="sidebar"></slot>
            </div>

            <div class="is-flex is-flex-column is-flex-grow-1">
                <b-progress
                    class="page-loading-bar has-margin-bottom-0"
                    size="is-medium"
                    type="is-warning"
                    v-if="loading"
                />

                <!-- Page header -->
                <slot name="header"></slot>

                <!-- Page body -->
                <div
                    class="is-flex is-flex-row is-flex-grow-1 has-overflow-y-scroll is-justify-content-center"
                >
                    <div class="container has-margin-all-3">
                        <slot></slot>
                    </div>

                    <slot name="overlays"></slot>
                </div>
            </div>
        </div>
    </div>
</template>

<style lang="sass">
.page-loading-bar
    left: 0px
    right: 0px
    
    progress
        height: 12px!important
</style>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';

@Component({
    name: 'page'
})
export default class Page extends Vue {
    @Prop({ default: false })
    loading!: boolean;

    @Prop({ default: 'is-light' })
    background!: string;

    get cssClass() {
        return `has-background-${
            this.background.split('-')[1]
        } has-overflow-y-hidden is-flex is-flex-column is-flex-grow-1`;
    }
}
</script>
