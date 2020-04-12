<template>
    <li :class="active ? 'is-active' : ''">
        <router-link :to="to">{{ name }}</router-link>
    </li>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { RawLocation } from 'vue-router';

@Component({
    name: 'breadcrumb'
})
export default class Breadcrumb extends Vue {
    @Prop({ required: true })
    name!: string;

    @Prop({ required: true })
    to!: RawLocation;

    @Prop({ default: false })
    active!: boolean;
}
</script>