<template>
    <div :class="cssClass">
        <!-- Standard View -->
        <div class="is-flex is-flex-column is-flex-grow-1" v-if="!$slots['sidebar']">
            <b-progress class="page-loading-bar has-margin-bottom-0" size="is-small" type="is-info" v-if="loading" />

            <slot name="header"></slot>

            <slot name="body">
                <div class="section has-padding-all-3">
                    <div class="container">
                        <slot></slot>
                    </div>
                </div>
            </slot>
        </div>

        <!-- Sidebar View -->
        <div class="is-flex is-flex-row" v-else>
            <div class="is-hidden-touch">
                <slot name="sidebar"></slot>
            </div>

            <div class="is-flex-grow-1">
                <b-progress
                    class="page-loading-bar has-margin-bottom-0"
                    size="is-small"
                    type="is-info"
                    v-if="loading"
                />

                <slot name="header"></slot>

                <slot name="body">
                    <div class="section has-padding-all-3">
                        <div class="container">
                            <slot></slot>
                        </div>
                    </div>
                </slot>
            </div>
        </div>
    </div>
</template>

<style lang="sass">
.page-loading-bar
    progress
        height: 6px!important
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
        return `has-background-${this.background.split('-')[1]}`;
    }
}
</script>
