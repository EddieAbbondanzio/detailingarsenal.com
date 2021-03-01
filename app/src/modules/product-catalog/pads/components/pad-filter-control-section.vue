<template>
    <b-collapse animation="slide" class="has-margin-bottom-3 has-padding-bottom-1 has-border-bottom-1-grey">
        <template #trigger="props">
            <div class="is-flex is-flex-row is-justify-content-space-between has-margin-bottom-1">
                <p class="title is-6 has-margin-bottom-0">
                    {{ label }}
                </p>
                <a>
                    <b-icon :icon="props.open ? 'minus' : 'plus'"> </b-icon>
                </a>
            </div>
        </template>

        <div>
            <slot></slot>
        </div>
    </b-collapse>
</template>
<script lang="ts">
import Vue from 'vue';
import Component from 'vue-class-component';
import { Prop } from 'vue-property-decorator';

@Component
export default class PadFilterControlSection extends Vue {
    @Prop({ required: true })
    label!: string;
}
</script>
