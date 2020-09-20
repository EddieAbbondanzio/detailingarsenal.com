<template>
    <page>
        <template v-slot:header>
            <page-header title="Write review" :description="description" :backButton="false" />
        </template>

        <input-form @submit="onSubmit" submitText="Create">
            <input-select label="Stars" rules="required" v-model="stars" :required="true">
                <option :value="null">Select stars</option>
                <option :value="1">1</option>
                <option :value="2">2</option>
                <option :value="3">3</option>
                <option :value="4">4</option>
                <option :value="5">5</option>
            </input-select>

            <b-slider>
                <b-slider-tick :value="2">REEE</b-slider-tick>
            </b-slider>
            <pad-cut-input v-model="cut" />
            <pad-finish-input v-model="finish" />

            <input-text-field
                label="Title"
                v-model="title"
                placeholder="Short summary in a few words"
                rules="required"
                :required="true"
            />
            <input-text-field
                type="textarea"
                label="Write your review"
                v-model="body"
                placeholder="Describe the pros, and cons, and all of the vivid details"
                rules="required"
                :required="true"
            />
        </input-form>
    </page>
</template>

<script lang="ts">
import { Pad, PadCut, PadFinish, Stars } from '@/api';
import { displayLoading } from '@/core';
import { Component, Vue, Prop } from 'vue-property-decorator';
import padStore from '../store/pad/pad-store';
import PadCutInput from '@/modules/product-catalog/pads/components/pad-cut-input.vue';
import PadFinishInput from '@/modules/product-catalog/pads/components/pad-finish-input.vue';

@Component({
    components: {
        PadCutInput,
        PadFinishInput
    }
})
export default class WriteReview extends Vue {
    get description() {
        return this.value?.label ?? '';
    }

    value: Pad | null = null;

    stars: Stars | null = null;
    cut: PadCut | null = null;
    finish: PadFinish | null = null;
    title: string | null = null;
    body: string | null = null;

    @displayLoading
    async created() {
        this.value = await padStore.getPadById(this.$route.params.id);
    }

    @displayLoading
    async onSubmit() {
        console.log('submitted');
    }
}
</script>