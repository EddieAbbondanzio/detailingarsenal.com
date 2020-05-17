<template>
    <div class="has-text-danger">
        <p v-if="flatErrors.length > 0">The following errors exist:</p>
        <ul>
            <li v-for="error in flatErrors" :key="error">{{ error }}</li>
        </ul>
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';

@Component({
    name: 'input-form-error-summary'
})
export default class InputFormErrorSummary extends Vue {
    @Prop({ default: {} })
    value!: {};

    get flatErrors() {
        return Object.values(this.value).flat();
    }
}
</script>