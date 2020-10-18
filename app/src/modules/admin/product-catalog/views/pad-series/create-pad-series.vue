<template>
    <page>
        <template v-slot:header>
            <page-header title="Create pad" :description="`Create a new pad series`">
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="Admin Panel" :to="{ name: 'adminPanel' }" />
                        <breadcrumb name="Product Catalog Panel" :to="{ name: 'productCatalogPanel' }" />
                        <breadcrumb name="Pads" :to="{ name: 'padSeries' }" />
                        <breadcrumb name="Create" :to="{ name: 'createPadSeries' }" :active="true" />
                    </breadcrumb-trail>
                </template>
            </page-header>
        </template>

        <input-form @submit="onSubmit" submitText="Create">
            <input-text-field
                label="Name"
                rules="required|max:32"
                :required="true"
                v-model="name"
                placeholder="Rupes"
            />

            <input-select label="Brand" v-model="brand" rules="required">
                <option :value="null">Select a brand</option>
                <option v-for="brand in brands" :key="brand.id" :value="brand">{{ brand.name }}</option>
            </input-select>

            <input-array
                title="Pads"
                :factory="() => ({ name: '', category: null, image: null })"
                v-slot="{ value }"
                v-model="pads"
            >
                <input-text-field
                    class="has-margin-x-1 has-margin-y-0"
                    type="text"
                    v-model="value.name"
                    label="Name"
                    rules="required|max:32"
                />

                <input-select
                    class="has-margin-x-1 has-margin-y-0"
                    label="Category"
                    rules="required"
                    v-model="value.category"
                >
                    <option :value="null">Select a category</option>
                    <option v-for="category in categories" :key="category[1]" :value="category[1]">{{
                        category[0]
                    }}</option>
                </input-select>

                <input-select
                    class="has-margin-x-1 has-margin-y-0"
                    label="Material"
                    rules="required"
                    v-model="value.material"
                >
                    <option :value="null">Select a material</option>
                    <option v-for="material in materials" :key="material[1]" :value="material[1]">
                        {{ material[0] }}
                    </option>
                </input-select>

                <input-select
                    class="has-margin-x-1 has-margin-y-0"
                    label="Texture"
                    rules="required"
                    v-model="value.texture"
                >
                    <option :value="null">Select a texture</option>
                    <option v-for="texture in textures" :key="texture[1]" :value="texture[1]">
                        {{ texture[0] }}
                    </option>
                </input-select>

                <input-image-upload label="Image" v-model="value.image" />
            </input-array>
        </input-form>
    </page>
</template>

<script lang="ts">
import { Component, Vue, Prop, Ref } from 'vue-property-decorator';
import { getModule } from 'vuex-module-decorators';
import ErrorMessage from '@/components/common/input/error-message.vue';
import ActionPage from '@/components/common/pages/action-page.vue';
import InputTextField from '@/core/components/input/input-text-field.vue';
import { toast, displayLoading, displayError } from '@/core';
import brandStore from '../../store/brand-store';
import { Brand, Image, Pad, PadCategory, PadCreateOrUpdate, PadMaterial } from '@/api';
import { PadTexture } from '@/api/product-catalog/pad-series/data-transfer-objects/pad-texture';

@Component
export default class CreatePadSeries extends Vue {
    get brands() {
        return brandStore.brands;
    }

    get categories() {
        return Object.entries(PadCategory);
    }

    get materials() {
        return Object.entries(PadMaterial);
    }

    get textures() {
        return Object.entries(PadTexture);
    }

    name: string = '';
    brand: Brand | null = null;
    pads: PadCreateOrUpdate[] = [];

    async created() {
        await brandStore.init();
    }

    @displayLoading
    public async onSubmit() {
        // const create = { name: this.name, brandId: this.brand!.id, pads: this.pads };
        // try {
        //     await padSeriesStore.create(create);
        //     toast(`Created new pad series ${create.name}`);
        //     this.$router.push({ name: 'padSeries' });
        // } catch (err) {
        //     if (err instanceof SpecificationError) {
        //         displayError(err);
        //     } else {
        //         throw err;
        //     }
        // }
    }
}
</script>
