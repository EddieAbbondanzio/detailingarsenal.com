<template>
    <b-field :label="hideLabel ? null : label" :class="required ? 'is-required' : ''">
        <template v-slot:label>
            <slot name="label"></slot>
        </template>

        <validation-provider
            :vid="vid"
            :name="label"
            :rules="rules"
            v-slot="{ errors, classes }"
            ref="validator"
        >
            <b-upload
                :type="type"
                :class="classes"
                accept="image/*"
                :disabled="disabled"
                @input="onInput"
                class="is-flex is-flex-row is-align-items-center"
            >
                <span class="file-cta">
                    <b-icon class="file-icon" icon="upload"></b-icon>
                    <span class="file-label">Click to upload</span>
                </span>
                <span class="file-name" v-if="value">{{ value.name }}</span>
                <a class="delete has-margin-left-1" v-if="value" @click.stop.prevent="onDelete"></a>
            </b-upload>
            <input-error-message v-if="!hideErrors" :text="errors[0]" />
        </validation-provider>
    </b-field>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { Image } from '@/api';
import { toBase64 } from '@/core/utils/to-base-64';

@Component({
    name: 'input-image-upload'
})
export default class InputImageUpload extends Vue {
    @Prop({ default: null })
    id!: string | null;

    @Prop()
    label!: boolean;

    @Prop()
    required!: boolean;

    @Prop()
    rules!: string;

    @Prop()
    value!: Image;

    @Prop({ default: false })
    disabled!: boolean;

    @Prop({ default: false })
    hideErrors!: boolean;

    @Prop({ default: false })
    hideLabel!: boolean;

    @Prop({ default: 'is-primary' })
    type!: string;

    get vid() {
        if (this.id != null) {
            return this.id;
        } else {
            return this.label;
        }
    }

    async onInput(val: File) {
        var data = await toBase64(val);
        this.$emit('input', new Image(val.name, data));
    }

    onDelete() {
        this.$emit('input', null);
    }
}
</script>