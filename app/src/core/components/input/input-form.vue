<template>
    <validation-observer
        tag="form"
        @submit.prevent
        v-slot="{ valid, pristine, errors, validated, handleSubmit }"
        v-disable-all="isLoading"
        class="box is-shadowless has-padding-all-2 has-padding-all-3-tablet"
    >
        <slot></slot>
        <!-- Submit, cancel button -->
        <div class="has-margin-top-3">
            <slot name="actions">
                <b-button
                    class="has-margin-right-1"
                    :type="submitType"
                    native-type="submit"
                    :loading="isLoading"
                    @click="handleSubmit(onSubmit)"
                >{{ submitText }}</b-button>

                <b-button
                    class="has-margin-left-1"
                    type="is-light"
                    @click="onCancel()"
                    :disabled="isLoading"
                >Cancel</b-button>
            </slot>

            <div v-if="debug">
                <div>
                    <span class="has-text-weight-bold">Valid:</span>
                    {{ valid }}
                </div>

                <div>
                    <span class="has-text-weight-bold">Pristine:</span>
                    {{ pristine }}
                </div>

                <div>
                    <span class="has-text-weight-bold">Validated:</span>
                    {{ validated }}
                </div>
                {{ errors }}
            </div>
        </div>

        <input-form-error-summary :value="errors" v-if="errorSummary" />
    </validation-observer>
</template>

<script lang="ts">
import { Component, Vue, Prop, Watch } from 'vue-property-decorator';
import appStore from '../../store/app-store';

@Component({
    name: 'input-form'
})
export default class InputForm extends Vue {
    get isLoading() {
        return appStore.loading;
    }

    @Prop({ default: false })
    debug!: boolean;

    @Prop({ default: 'Submit' })
    submitText!: string;

    @Prop({ default: 'is-success' })
    submitType!: string;

    @Prop({ default: false })
    errorSummary!: boolean;

    @Prop({ default: false })
    preventCancelDefault!: boolean;

    onSubmit() {
        this.$emit('submit');
    }

    onCancel() {
        if (!this.preventCancelDefault) {
            this.$router.go(-1);
        }

        this.$emit('cancel');
    }
}
</script>