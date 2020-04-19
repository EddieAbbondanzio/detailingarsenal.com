<template>
    <b-field grouped>
        <slot></slot>
    </b-field>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';

@Component({
    name: 'input-group'
})
export default class InputGroup extends Vue {}
</script>