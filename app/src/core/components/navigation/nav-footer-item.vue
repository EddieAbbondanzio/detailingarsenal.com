<template>
    <div class="level-item">
        <router-link
            :exact="exact"
            class="is-flex is-flex-column is-align-items-center has-text-dark"
            :to="to"
        >
            <b-icon style :icon="icon" v-if="icon != null" />
            <span class="is-size-7">{{ text }}</span>
        </router-link>
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { RawLocation } from 'vue-router';

@Component({ name: 'nav-footer-item' })
export default class NavFooterItem extends Vue {
    @Prop({ required: true })
    to!: RawLocation;

    @Prop({ required: true })
    text!: string;

    @Prop()
    icon!: string | null;

    @Prop({ default: true })
    exact!: boolean;
}
</script>