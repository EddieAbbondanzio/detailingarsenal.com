<template>
    <progress
        class="progress is-inline-block is-small"
        type="is-dark"
        :value="value"
        :min="0"
        :max="10"
    ></progress>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { PadFinish } from '@/api';

@Component
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