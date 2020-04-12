<template>
    <div>
        <b-button
            class="is-hidden-tablet"
            type="is-danger"
            size="is-medium"
            :icon-left="icon"
            @click="$emit('click')"
            :disabled="disabled"
            :loading="loading"
            :outlined="outlined"
            :to="to"
            :tag="tag"
        ></b-button>
        <b-button
            class="is-hidden-mobile"
            type="is-danger"
            :icon-left="icon"
            @click="$emit('click')"
            :disabled="disabled"
            :loading="loading"
            :outlined="outlined"
            :to="to"
            :tag="tag"
        >{{ text }}</b-button>
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { RawLocation } from 'vue-router';

import ButtonMixin from '@/core/mixins/button-mixin';

@Component({
    name: 'delete-button'
})
export default class DeleteButton extends ButtonMixin {
    @Prop({ default: 'delete' })
    icon!: string;

    @Prop({ default: 'Delete' })
    text!: string;
}
</script>

