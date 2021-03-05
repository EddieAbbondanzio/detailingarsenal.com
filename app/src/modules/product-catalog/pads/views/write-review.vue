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

            <pad-cut-input v-model="cut" />
            <pad-finish-input v-model="finish" />

            <input-text-field
                label="Title"
                v-model="title"
                placeholder="Short summary in a few words"
                rules="required|max:64"
                :required="true"
                maxLength="64"
            />
            <input-text-field
                type="textarea"
                label="Body"
                v-model="body"
                placeholder="Describe the pros, and cons, and all of the vivid details"
                rules="required|max:10000"
                :required="true"
                maxLength="10000"
            />
        </input-form>
    </page>
</template>

<script lang="ts">
import { PadCut, PadFinish, Stars } from '@/api/product-catalog';
import { displayError, displayLoading } from '@/core';
import { Component, Vue, Prop } from 'vue-property-decorator';
import padStore from '../store/pad/pad-store';
import PadCutInput from '@/modules/product-catalog/pads/components/pad-cut-input.vue';
import PadFinishInput from '@/modules/product-catalog/pads/components/pad-finish-input.vue';
import reviewStore from '../store/review/review-store';
import { Pad } from '@/api/product-catalog/data-transfer-objects/pad';

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
        this.value = padStore.pads.values.find(p => p.id == this.$route.params.padId)!;

        // Pull in the pad if we can't find it.
        if (this.value == null) {
            await padStore.get(this.$route.params.padSeriesId);
            this.value = padStore.pads.values.find(p => p.id == this.$route.params.padId)!;
        }
    }

    @displayLoading
    async onSubmit() {
        try {
            await reviewStore.create({
                padId: this.value!.id,
                stars: this.stars!,
                cut: this.cut,
                finish: this.finish,
                title: this.title!,
                body: this.body!
            });

            // Redirect to the pad they just wrote a review for.
            this.$router.push({ name: 'pad', params: { id: this.value!.id } });
        } catch (e) {
            displayError(e);
        }
    }
}
</script>
