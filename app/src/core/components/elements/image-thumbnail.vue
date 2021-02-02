<template>
    <img :class="classes" :src="imageUrl" />
</template>

<style lang="sass">
.is-small
    height: 32px!important
.is-medium
    height: 64px!important
.is-large
    height: 100px!important
</style>

<script lang="ts">
import Vue from 'vue';
import Component from 'vue-class-component';
import { Prop } from 'vue-property-decorator';

@Component
export default class ImageThumbnail extends Vue {
    get classes() {
        return ['image-thumbnail', this.size];
    }

    get imageUrl() {
        if (typeof this.value == 'string') {
            return `${process.env.VUE_APP_API_DOMAIN}/image/${this.value}/thumbnail`;
        } else {
            return (this.value as any).data;
        }
    }

    @Prop({ default: 'is-small' })
    size!: string;

    @Prop()
    value!: File | string;

    getDataUrl(file: File) {
        return new Promise((resolve, reject) => {
            const reader = new FileReader();
            reader.readAsDataURL(file);
            reader.onload = () => resolve(reader.result);
            reader.onerror = error => reject(error);
        });
    }
}
</script>
