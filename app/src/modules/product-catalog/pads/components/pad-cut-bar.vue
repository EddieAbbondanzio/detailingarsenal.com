<template>
    <div class="pad-cut-bar is-flex is-flex-row is-align-items-center" v-if="value != null">
        <progress
            class="progress is-inline-block is-small has-margin-bottom-0 has-margin-right-1"
            type="is-dark"
            :value="value"
            :min="0"
            :max="10"
        ></progress>
        <span class="has-text-grey" style="white-space: nowrap">{{ value }} / 10</span>
    </div>
    <span class="has-text-grey" v-else>N/A</span>
</template>

<style lang="sass" scoped>
.pad-cut-bar
    max-width: 190px
</style>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { PadCut } from '@/api';

@Component({ name: 'pad-cut-bar' })
export default class PadCutBar extends Vue {
    get type() {
        switch (this.value) {
            case 0:
            case 1:
                return 'is-green';
            case 2:
            case 3:
            case 4:
                return 'is-yellow';
            case 5:
            case 6:
            case 7:
                return 'is-orange';
            case 8:
            case 9:
            case 10:
                return 'is-red';
        }
    }

    /**
     * Pad cut level
     */
    @Prop()
    value!: PadCut;
}
</script>