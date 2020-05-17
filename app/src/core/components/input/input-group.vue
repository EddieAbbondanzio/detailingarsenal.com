<template>
    <b-field class="input-group" grouped :group-multiline="multiline">
        <slot></slot>
    </b-field>
</template>

<style lang="sass">
.input-group
    .field
        flex-grow: 1!important

+mobile
    .input-group.is-grouped-multiline
        .field:last-child
            margin-bottom: 12px!important
</style>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';

@Component({
    name: 'input-group'
})
export default class InputGroup extends Vue {
    @Prop({ default: false })
    multiline!: boolean;
}
</script>