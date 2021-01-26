<template>
    <page>
        <template v-slot:header>
            <page-header :title="`Update ${name}`" :description="`Update a pad series`">
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="Admin panel" :to="{ name: 'adminPanel' }" />
                        <breadcrumb name="Product catalog panel" :to="{ name: 'productCatalogPanel' }" />
                        <breadcrumb name="Pad series" :to="{ name: 'padSeries' }" />
                        <breadcrumb :name="name" :to="{ name: 'padSeries', params: $route.params }" :active="true" />
                        <breadcrumb name="Update" :to="$route" :active="true" />
                    </breadcrumb-trail>
                </template>
            </page-header>
        </template>

        <input-form @submit="onSubmit" submitText="Update">
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

            <b-field label="Polisher Type(s)">
                <input-checkbox v-model="polisherTypes" native-value="dual_action" label="Dual Action" />
                <input-checkbox v-model="polisherTypes" native-value="long_throw" label="Long Throw" />
                <input-checkbox v-model="polisherTypes" native-value="forced_rotation" label="Forced Rotation" />
                <input-checkbox v-model="polisherTypes" native-value="rotary" label="Rotary" />
                <input-checkbox v-model="polisherTypes" native-value="mini" label="Mini" />
            </b-field>

            <input-array title="Sizes" :factory="() => ({})" v-model="sizes" v-slot="{ value }">
                <measurement-input label="Diameter" v-model="value.diameter" rules="required" :required="true" />
                <measurement-input label="Thickness" v-model="value.thickness" />
            </input-array>

            <input-array title="Colors" :factory="padColorUpdateFactory" v-model="colors">
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

                    <input-select
                        class="has-margin-x-1 has-margin-y-0"
                        label="Material"
                        rules="required"
                        v-model="value.material"
                    >
                        <option :value="null">Select a material</option>
                        <option v-for="m in materials" :key="m[1]" :value="m[1]">
                            {{ m[0] }}
                        </option>
                    </input-select>

                    <input-select
                        class="has-margin-x-1 has-margin-y-0"
                        label="Texture"
                        rules="required"
                        v-model="value.texture"
                    >
                        <option :value="null">Select a texture</option>
                        <option v-for="t in textures" :key="t[1]" :value="t[1]">
                            {{ t[0] }}
                        </option>
                    </input-select>

                    <input-image-upload v-if="!isExistingImage(value.image)" label="Image" v-model="value.image" />
                    <div class="has-margin-top-4 is-align-self-center" v-else>
                        <img style="max-height: 40px !important" :src="getThumbnailUrl(value.image)" />
                        <a class="delete" @click="onDeleteImage(value)"></a>
                    </div>
                </template>

                <template v-slot:detail="{ value: color }">
                    <input-array title="Options" v-model="color.options">
                        <template v-slot="{ value }">
                            <input-select label="Size" v-model="value.padSizeId" rules="required">
                                <option :value="null">Select a size</option>
                                <option
                                    v-for="(size, i) in sizes"
                                    :key="i"
                                    :value="size.id"
                                    :disabled="isSizeDisabled(size, value, color)"
                                >
                                    {{ size.diameter.amount.toString() + size.diameter.unit }}
                                </option>
                            </input-select>

                            <input-text-field label="Part Number" v-model="value.partNumber" />
                        </template>
                    </input-array>
                </template>
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
import {
    Brand,
    Image,
    PadCategory,
    PadMaterial,
    PadTexture,
    PolisherType,
    SpecificationError,
    PadOption,
    PadSizeCreateOrUpdate,
    PadSize,
    PadSeriesUpdateRequest,
    PadColor,
    PadColorCreateOrUpdate,
    PadOptionCreateOrUpdate
} from '@/api';
import padStore from '@/modules/product-catalog/pads/store/pad/pad-store';
import adminPadStore from '../../store/admin-pad-store';
import MeasurementInput from '@/modules/shared/components/measurement-input.vue';

@Component({
    components: {
        MeasurementInput
    }
})
export default class UpdatePadSeries extends Vue {
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
    polisherTypes: PolisherType[] = [];
    sizes: PadSizeCreateOrUpdate[] = [];
    colors: PadColorCreateOrUpdate[] = [];

    @displayLoading
    async created() {
        await brandStore.init();
        await adminPadStore.init();

        const padSeries = adminPadStore.series.find(s => s.id == this.$route.params.id);

        if (padSeries == null) {
            this.$router.go(-1);
            return;
        }

        this.name = padSeries.name;
        this.brand = padSeries.brand;
        this.polisherTypes = padSeries.polisherTypes;
        this.sizes = padSeries.sizes.map(s => ({
            id: s.id,
            diameter: s.diameter,
            thickness: s.thickness!
        }));
        this.colors = padSeries.colors.map(c => ({
            id: c.id,
            name: c.name,
            category: c.category,
            material: c.material,
            texture: c.texture,
            image: c.image, // Existing images are just ids to real images.
            options: c.options.map(o => ({
                padSizeIndex: null,
                padSizeId: o.padSizeId,
                partNumber: o.partNumber
            }))
        }));
    }

    @displayLoading
    public async onSubmit() {
        const update: PadSeriesUpdateRequest = {
            id: this.$route.params.id,
            name: this.name,
            polisherTypes: this.polisherTypes,
            brandId: this.brand!.id,
            sizes: this.sizes.map(s => ({
                id: s.id,
                diameter: s.diameter,
                thickness: s.thickness?.amount != null ? s.thickness : null
            })),
            colors: this.colors
        };

        try {
            await adminPadStore.update(update);
            toast(`Updated pad series ${update.name}`);
            this.$router.push({ name: 'padSeries' });
        } catch (err) {
            displayError(err);
        }
    }

    padColorUpdateFactory(): PadColorCreateOrUpdate {
        return {
            id: null,
            name: '',
            category: null!,
            material: null!,
            texture: null!,
            options: [],
            image: null
        };
    }

    /**
     * Don't allow a size to be picked on an option if it's already used by another
     * option on the same color.
     */
    isSizeDisabled(size: PadSizeCreateOrUpdate, option: PadOptionCreateOrUpdate, color: PadColorCreateOrUpdate) {
        const sizesUsedAlready = color.options.map(o => o.padSizeIndex!).map(i => this.sizes[i]);

        return !sizesUsedAlready.every(s => s != size);
    }

    isExistingImage(image: Image | string | null) {
        return image != null && !(image instanceof Image);
    }

    onDeleteImage(value: PadColorCreateOrUpdate) {
        value.image = null;
    }

    getThumbnailUrl(id: string) {
        return `${process.env.VUE_APP_API_DOMAIN}/image/${id}/thumb`;
    }
}
</script>
