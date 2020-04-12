<template>
    <a
        :href="`tel:${value}`"
        class="has-margin-right-3 is-flex is-flex-row is-align-items-center"
        :disabled="disabled"
    >
        <b-icon class="has-margin-right-1" icon="phone" type="is-dark" />
        <span class="has-text-grey">{{ value }}</span>
    </a>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';

@Component({
    name: 'phone'
})
export default class Phone extends Vue {
    @Prop()
    value!: string;

    @Prop({ default: false })
    disabled!: boolean;
}
</script>