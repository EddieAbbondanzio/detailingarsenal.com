<template>
    <page>
        <template v-slot:header>
            <page-header title="Create pad" :description="`Create a new pad series`">
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="Admin panel" :to="{ name: 'adminPanel' }" />
                        <breadcrumb name="Product catalog panel" :to="{ name: 'productCatalogPanel' }" />
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

            <input-select class="has-margin-x-1 has-margin-y-0" label="Material" rules="required" v-model="material">
                <option :value="null">Select a material</option>
                <option v-for="m in materials" :key="m[1]" :value="m[1]">
                    {{ m[0] }}
                </option>
            </input-select>

            <input-select class="has-margin-x-1 has-margin-y-0" label="Texture" rules="required" v-model="texture">
                <option :value="null">Select a texture</option>
                <option v-for="t in textures" :key="t[1]" :value="t[1]">
                    {{ t[0] }}
                </option>
            </input-select>

            <input-tagger
                class="has-margin-x-1 has-margin-y-0"
                label="Polisher Type(s)"
                rules="required"
                v-model="polisherTypes"
                autocomplete
                :data="allPolisherTypes"
            />

            <input-array title="Sizes" :factory="() => ({})" v-model="sizes" v-slot="{ value }">
                <measurement-input label="Diameter" v-model="value.diameter" />
                <measurement-input label="Thickness" v-model="value.thickness" />
            </input-array>

            <input-array title="Colors" :factory="padColorCreateOrUpdateFactory" v-model="pads">
                <template v-slot="{ value }">
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
                        <option v-for="category in categories" :key="category[1]" :value="category[1]">
                            {{ category[0] }}
                        </option>
                    </input-select>

                    <input-image-upload label="Image" v-model="value.image" />
                </template>

                <template v-slot:detail="{ value }">
                    <input-array title="Options" v-model="value.options">
                        <template v-slot="{ value }">
                            <input-select label="Size" v-model="value.padSizeId" rules="required">
                                <option :value="null">Select a size</option>
                                <option v-for="size in sizes" :key="size.id" :value="size">{{ size.diameter }}</option>
                            </input-select>

                            <input-text-field v-model="value.partNumber" />
                        </template>
                    </input-array>
                </template>
            </input-array>
        </input-form>

        {{ pads[0] }}
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
import {
    Brand,
    Image,
    Pad,
    PadCategory,
    PadCreateOrUpdate,
    PadMaterial,
    PadTexture,
    PadSeriesSize,
    PolisherType,
    SpecificationError,
    PadColorCreateOrUpdate,
    PadOption,
} from '@/api';
import padStore from '@/modules/product-catalog/pads/store/pad/pad-store';
import adminPadStrore from '../../store/admin-pad-store';
import MeasurementInput from '@/modules/shared/components/measurement-input.vue';

@Component({
    components: {
        MeasurementInput,
    },
})
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

    get allPolisherTypes() {
        return Object.values(PolisherType);
    }

    name: string = '';
    brand: Brand | null = null;
    material: PadMaterial | null = null;
    texture: PadTexture | null = null;
    polisherTypes: PolisherType[] = [];
    pads: PadCreateOrUpdate[] = [];
    sizes: PadSeriesSize[] = [];

    async created() {
        await brandStore.init();
    }

    @displayLoading
    public async onSubmit() {
        console.log(this.sizes[0].diameter);

        // const create = { name: this.name, brandId: this.brand!.id, pads: this.pads, sizes: this.sizes };
        // try {
        //     console.log(this.sizes);
        //     await adminPadStrore.create(create);
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

    padColorCreateOrUpdateFactory(): PadColorCreateOrUpdate {
        return {
            name: '',
            category: null!,
            options: [],
        };
    }
}
</script>
