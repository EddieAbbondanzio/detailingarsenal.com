<template>
    <div class="is-flex is-flex-row is-align-items-center">
        <b-rate v-model="value" class="has-margin-all-0" :size="size" :disabled="readOnly" />
        <span class="is-size-6" v-if="!hideCount">({{ count }})</span>
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { Stars } from '@/api/product-catalog';

@Component({ name: 'stars' })
export default class StarsComponent extends Vue {
    @Prop()
    value!: Stars | null;

    @Prop({ default: 0 })
    count!: number;

    @Prop({ default: false })
    hideCount!: boolean;

    @Prop({ default: false })
    readOnly!: boolean;

    @Prop()
    size!: string;

    getIcon(i: number) {
        return this.value == null || i > this.value ? 'star-outline' : 'star';
    }

    getColor(i: number) {
        return this.value == null || i > this.value ? 'is-dark' : 'is-orange';
    }
}
</script>
