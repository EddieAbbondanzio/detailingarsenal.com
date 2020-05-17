<template>
    <span class="has-text-danger has-margin-y-1 is-flex is-flex-row" v-if="text">
        <b-icon class="has-margin-right-1" icon="alert" />
        {{ text }}
    </span>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';

@Component({
    name: 'input-error-message'
})
export default class InputErrorMessage extends Vue {
    @Prop()
    public text!: string;
}
</script>