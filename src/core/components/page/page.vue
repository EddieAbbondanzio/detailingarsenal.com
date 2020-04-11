<template>
    <div :class="cssClass">
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
        return `is-flex is-flex-column is-flex-grow-1 has-background-${this.background.split('-')[1]}`;
    }
}
</script>