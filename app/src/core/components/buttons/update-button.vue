<template>
    <div>
        <b-button
            class="is-hidden-tablet"
            type="is-text"
            size="is-medium"
            :icon-left="icon"
            @click="$emit('click')"
            :outlined="outlined"
            :loading="loading"
            :to="to"
            :tag="tag"
        ></b-button>
        <b-button
            class="is-hidden-mobile"
            type="is-primary"
            :icon-left="icon"
            @click="$emit('click')"
            :outlined="outlined"
            :loading="loading"
            :to="to"
            :tag="tag"
            >{{ text }}</b-button
        >
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { RawLocation } from 'vue-router';
import ButtonMixin from '../../mixins/button-mixin';

@Component({
    name: 'update-button',
})
export default class UpdateButton extends ButtonMixin {
    @Prop({ default: 'Update' })
    text!: string;

    @Prop({ default: 'pencil' })
    icon!: string;
}
</script>