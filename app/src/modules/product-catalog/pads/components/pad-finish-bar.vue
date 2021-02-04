<template>
    <div class="pad-finish-bar is-flex is-flex-row is-align-items-center" v-if="value != null">
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
.pad-finish-bar
    max-width: 190px


    progress
        height: 12px!important
</style>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { PadFinish } from '@/api';

@Component({ name: 'pad-finish-bar' })
export default class PadFinishBar extends Vue {
    get type() {
        switch (this.value) {
            case 10:
            case 9:
                return 'is-green';
            case 8:
            case 7:
            case 6:
                return 'is-yellow';
            case 5:
            case 4:
            case 3:
                return 'is-orange';
            case 2:
            case 1:
            case 0:
                return 'is-red';
        }
    }

    /**
     * Pad Finish level
     */
    @Prop()
    value!: PadFinish;
}
</script>
