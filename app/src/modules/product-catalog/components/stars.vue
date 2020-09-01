<template>
    <div>
        <b-icon v-for="i in 5" :key="i" :icon="getIcon(i)" size="is-small" :type="getColor(i)" />
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { Stars } from '@/api';

@Component
export default class StarsComponent extends Vue {
    @Prop()
    value!: Stars | null;

    getIcon(i: number) {
        return this.value == null || i > this.value ? 'star-outline' : 'star';
    }

    getColor(i: number) {
        return this.value == null || i > this.value ? 'is-dark' : 'is-orange';
    }
}
</script>