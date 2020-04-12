<template>
    <ul :class="cssClasses">
        <slot>
            <p class="has-padding-all-4 has-text-centered has-text-dark-grey">{{ emptyText }}</p>
        </slot>
    </ul>
</template>

<style lang="sass" scoped>
ul.is-regular
    li
        border-radius: $radius-large
        margin-bottom: $gap * 0.25
        background-color: $white
        color: $grey-dark

ul.is-compact
    border-radius: $radius-large
    background-color: $white
    color: $grey-dark

    li
        border-bottom: 1px solid $light
</style>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';

@Component({
    name: 'list'
})
export default class List extends Vue {
    @Prop({ default: "There's nothing here!" })
    emptyText!: string;

    @Prop({ default: 'is-regular' })
    size!: string;

    get cssClasses() {
        return `${this.size}`;
    }
}
</script>