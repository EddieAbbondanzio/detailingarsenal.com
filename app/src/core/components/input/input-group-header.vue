<template>
    <div>
        <p class="is-size-6 has-text-weight-bold has-margin-bottom-2">{{ text }}</p>
        <hr class="has-margin-top-0 has-padding-y-0 has-margin-bottom-3" />
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';

@Component({
    name: 'input-group-header'
})
export default class InputGroupHeader extends Vue {
    @Prop({ default: '' })
    text!: string;
}
</script>