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
            <input-slider
                label="Cut"
                v-model="cut"
                :min="0"
                :max="10"
                :ticks="[
                {value: 0, label: 0},
                {value: 1, label: 1},
                {value: 2, label: 2},
                {value: 3, label: 3},
                {value: 4, label: 4},
                {value: 5, label: 5},
                {value: 6, label: 6},
                {value: 7, label: 7},
                {value: 8, label: 8},
                {value: 9, label: 9},
                {value: 10, label: 10},
                ]"
            />
            <input-slider label="Finish" v-model="finish" :min="0" :max="10" />

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

@Component
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