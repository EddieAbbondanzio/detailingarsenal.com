<template>
    <page>
        <template v-slot:header>
            <page-header title="Create pad series" :description="`Create a new pad series`">
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="Admin panel" :to="{ name: 'adminPanel' }" />
                        <breadcrumb name="Product catalog panel" :to="{ name: 'productCatalogPanel' }" />
                        <breadcrumb name="Pad series" :to="{ name: 'padSeries' }" />
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

            <input-array title="Colors" :factory="padColorCreateFactory" v-model="colors">
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

                <template v-slot:detail="{ value: color }">
                    <input-array title="Options" v-model="color.options">
                        <template v-slot="{ value }">
                            <input-select label="Size" v-model="value.padSizeIndex" rules="required">
                                <option :value="null">Select a size</option>
                                <option
                                    v-for="(size, i) in sizes"
                                    :key="i"
                                    :value="i"
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
    PadColorCreate,
    PadSizeCreateOrUpdate,
    PadSize,
    PadOptionCreate,
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
    sizes: PadSizeCreateOrUpdate[] = [];
    colors: PadColorCreate[] = [];

    async created() {
        await brandStore.init();
    }

    @displayLoading
    public async onSubmit() {
        const create = {
            name: this.name,
            texture: this.texture!,
            material: this.material!,
            polisherTypes: this.polisherTypes,
            brandId: this.brand!.id,
            sizes: this.sizes,
            colors: this.colors,
        };

        try {
            await adminPadStrore.create(create);
            toast(`Created new pad series ${create.name}`);
            this.$router.push({ name: 'padSeries' });
        } catch (err) {
            if (err instanceof SpecificationError) {
                displayError(err);
            } else {
                throw err;
            }
        }
    }

    padColorCreateFactory(): PadColorCreate {
        return {
            name: '',
            category: null!,
            options: [],
        };
    }

    /**
     * Don't allow a size to be picked on an option if it's already used by another
     * option on the same color.
     */
    isSizeDisabled(size: PadSizeCreateOrUpdate, option: PadOptionCreate, color: PadColorCreate) {
        const sizesUsedAlready = color.options.map((o) => o.padSizeIndex).map((i) => this.sizes[i]);

        return !sizesUsedAlready.every((s) => s != size);
    }
}
</script>
