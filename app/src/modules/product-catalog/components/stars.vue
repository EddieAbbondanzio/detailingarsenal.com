<template>
    <div class="is-flex is-flex-row is-align-items-center">
        <b-icon v-for="i in 5" :key="i" :icon="getIcon(i)" size="is-small" :type="getColor(i)" />
        <span class="has-margin-left-1">({{ count }})</span>
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { Stars } from '@/api';

@Component
export default class StarsComponent extends Vue {
    @Prop()
    value!: Stars | null;

    @Prop({ default: 0 })
    count!: number;

    getIcon(i: number) {
        return this.value == null || i > this.value ? 'star-outline' : 'star';
    }

    getColor(i: number) {
        return this.value == null || i > this.value ? 'is-dark' : 'is-orange';
    }
}
</script>