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
                v-focus
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

            <!-- Size Table -->
            <b-field label="Sizes">
                <b-table :data="sizes" class="is-flex" narrowed>
                    <b-table-column field="diameter" label="Diameter" v-slot="props">
                        <measurement-input :value="props.row.diameter" rules="required" :required="true" />
                    </b-table-column>
                    <b-table-column field="thickness" label="Thickness" v-slot="props">
                        <measurement-input :value="props.row.thickness" />
                    </b-table-column>
                    <b-table-column v-slot="props" class="has-vertical-align-middle">
                        <b-button type="is-danger" @click="onDeleteSize(props.index)" size="is-small">Delete</b-button>
                    </b-table-column>
                </b-table>
            </b-field>
            <b-button
                type="is-text"
                @click="sizes.push({ id: null, diameter: { amount: null, unit: 'in' }, thickness: null })"
                >Add another</b-button
            >
            <!-- Pad Table -->
            <b-field label="Pads">
                <b-table :data="pads" narrowed class="pad-table" detailed>
                    <b-table-column label="Name" field="name" v-slot="props">
                        {{ props.row.name }}
                    </b-table-column>

                    <b-table-column label="Category" field="category" v-slot="props">
                        {{ props.row.category | uppercaseFirst }}
                    </b-table-column>

                    <b-table-column field="material" label="Material" v-slot="props">
                        {{ props.row.material | uppercaseFirst }}
                    </b-table-column>

                    <b-table-column field="texture" label="Texture" v-slot="props">
                        {{ props.row.texture | uppercaseFirst }}
                    </b-table-column>

                    <b-table-column field="color" label="Color" v-slot="props">
                        {{ props.row.color | uppercaseFirst }}
                    </b-table-column>

                    <b-table-column field="hasCenterHole" label="Center hole" v-slot="props">
                        {{ props.row.hasCenterHole }}
                    </b-table-column>

                    <b-table-column field="image" label="Image" v-slot="props">
                        <image-thumbnail :value="props.row.image" />
                    </b-table-column>
                    <b-table-column v-slot="props" centered class="has-vertical-align-middle">
                        <div class="is-flex is-flex-row">
                            <b-button
                                class="has-margin-right-2"
                                type="is-primary"
                                @click="onPadEdit(props.index)"
                                size="is-small"
                                >Edit</b-button
                            >
                            <b-button type="is-danger" @click="pads.splice(props.index, 1)" size="is-small"
                                >Delete</b-button
                            >
                        </div>
                    </b-table-column>

                    <template #detail="props">
                        <ul v-for="option in props.row.options" :key="option.padSizeIndex">
                            <li>
                                <span class="has-text-weight-bold">{{
                                    sizes[option.padSizeIndex].diameter | measurement
                                }}</span>
                                <span>:&nbsp;</span>

                                <p
                                    class="is-inline-block"
                                    v-for="(partNumber, i) in option.partNumbers"
                                    :key="partNumber.value"
                                >
                                    <span>{{ partNumber.value }}</span
                                    ><span class="has-text-grey" v-if="partNumber.notes">({{ partNumber.notes }})</span>
                                    <span class="has-margin-right-1" v-if="i < option.partNumbers.length - 1">,</span>
                                </p>
                            </li>
                        </ul>
                    </template>
                </b-table>
            </b-field>
            <b-button type="is-text" @click="onPadAddAnother">Add another</b-button>
        </input-form>

        <!-- Pad Modal -->
        <b-modal v-model="isPadModalActive" has-modal-card v-if="modalPad != null">
            <validation-observer ref="modalValidator" tag="div" class="modal-card">
                <div class="modal-card-head">
                    <p class="modal-card-title">Enter new pad info</p>
                </div>
                <div class="modal-card-body has-padding-bottom-3">
                    <input-text-field
                        class="has-margin-x-1 has-margin-y-0"
                        type="text"
                        label="Name"
                        v-model="modalPad.name"
                        :required="true"
                        rules="required|max:32"
                        v-focus
                    />

                    <input-select
                        label="Category"
                        class="has-margin-x-1 has-margin-y-0"
                        rules="required"
                        :required="true"
                        v-model="modalPad.category"
                    >
                        <option :value="null">Select a category</option>
                        <option v-for="category in categories" :key="category[1]" :value="category[1]">
                            {{ category[0] }}
                        </option>
                    </input-select>

                    <input-select label="Material" class="has-margin-x-1 has-margin-y-0" v-model="modalPad.material">
                        <option :value="null">Select a material</option>
                        <option v-for="m in materials" :key="m[1]" :value="m[1]">
                            {{ m[0] }}
                        </option>
                    </input-select>

                    <input-select label="Texture" class="has-margin-x-1 has-margin-y-0" v-model="modalPad.texture">
                        <option :value="null">Select a texture</option>
                        <option v-for="t in textures" :key="t[1]" :value="t[1]">
                            {{ t[0] }}
                        </option>
                    </input-select>

                    <input-select label="Color" class="has-margin-x-1 has-margin-y-0" v-model="modalPad.color">
                        <option :value="null">Select a color</option>
                        <option v-for="c in colors" :key="c[1]" :value="c[1]">
                            {{ c[0] }}
                        </option>
                    </input-select>

                    <input-checkbox
                        label="Has center hole"
                        v-model="modalPad.hasCenterHole"
                        class="has-margin-all-2 has-margin-top-3"
                    />

                    <input-image-upload label="Image" v-model="modalPad.image" />

                    <b-field label="Options">
                        <b-table :data="modalPad.options" detailed>
                            <b-table-column label="Size" field="padSizeIndex" v-slot="props">
                                <input-select
                                    v-model="props.row.padSizeIndex"
                                    rules="required"
                                    label="Size"
                                    :hideLabel="true"
                                >
                                    <option :value="null">Select a size</option>
                                    <option
                                        v-for="(size, i) in sizes"
                                        :key="i"
                                        :value="i"
                                        :disabled="isSizeDisabled(size, modalPad)"
                                    >
                                        {{
                                            size.diameter.amount == null
                                                ? ''
                                                : size.diameter.amount.toString() + size.diameter.unit
                                        }}
                                    </option>
                                </input-select>
                            </b-table-column>

                            <b-table-column centered v-slot="{ index }">
                                <b-button type="is-danger" @click="modalPad.options.splice(index, 1)">Delete</b-button>
                            </b-table-column>

                            <template #detail="props">
                                <b-table :data="props.row.partNumbers">
                                    <b-table-column label="Part Number" v-slot="props">
                                        <input-text-field
                                            v-model="props.row.value"
                                            placeholder="PART-NUMBER-123"
                                            label="Part number"
                                            :hideLabel="true"
                                            rules="required|max:64"
                                            v-focus
                                        />
                                    </b-table-column>
                                    <b-table-column label="Notes" v-slot="props">
                                        <input-text-field
                                            v-model="props.row.notes"
                                            placeholder="2 pack"
                                            rules="max:128"
                                        />
                                    </b-table-column>
                                    <b-table-column centered v-slot="{ index }">
                                        <b-button type="is-danger" @click="props.row.partNumbers.splice(index, 1)"
                                            >Delete</b-button
                                        >
                                    </b-table-column>
                                </b-table>
                                <b-button
                                    type="is-text"
                                    @click="props.row.partNumbers.push({ value: null, notes: null })"
                                    >Add another</b-button
                                >
                            </template>
                        </b-table>
                    </b-field>
                    <b-button type="is-text" @click="onOptionAddAnother">Add another</b-button>
                </div>

                <footer class="modal-card-foot">
                    <b-button label="Done" type="is-success" @click="onPadAddDone" />
                    <b-button label="Cancel" @click="onPadAddCancel" />
                </footer>
            </validation-observer>
        </b-modal>
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
    Pad,
    PadCreateOrUpdate,
    PadOptionCreateOrUpdate
} from '@/api';
import padStore from '@/modules/product-catalog/pads/store/pad/pad-store';
import adminPadStore from '../../store/admin-pad-store';
import MeasurementInput from '@/modules/shared/components/measurement-input.vue';
import { PadColor } from '@/api/product-catalog/data-transfer-objects/pad-color';
import { measurement } from '@/modules/shared/filters/measurement';

@Component({
    components: {
        MeasurementInput
    },
    filters: {
        measurement
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

    get colors() {
        return Object.entries(PadColor);
    }

    get allPolisherTypes() {
        return Object.values(PolisherType);
    }

    name: string = '';
    brand: Brand | null = null;
    polisherTypes: PolisherType[] = [];
    sizes: PadSizeCreateOrUpdate[] = [];
    pads: PadCreateOrUpdate[] = [];
    isPadModalActive: boolean = false;
    isPadModalInEditMode: boolean = false;
    modalPad: PadCreateOrUpdate | null = null;

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
        this.polisherTypes = padSeries.polisherTypes ?? [];
        this.sizes = padSeries.sizes.map(s => ({
            id: s.id,
            diameter: s.diameter,
            thickness: s.thickness!
        }));
        this.pads = padSeries.pads.map(c => ({
            id: c.id,
            name: c.name,
            category: c.category,
            material: c.material,
            texture: c.texture,
            color: c.color,
            hasCenterHole: c.hasCenterHole,
            image: c.image, // Existing images are just ids to real images.
            options: c.options.map<PadOptionCreateOrUpdate>(o => ({
                padSizeIndex: this.sizes.findIndex(s => s.id == o.padSizeId),
                partNumbers: o.partNumbers ?? []
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
            pads: this.pads
        };

        try {
            await adminPadStore.update(update);
            toast(`Updated pad series ${update.name}`);
            this.$router.push({ name: 'padSeriesDetails' });
        } catch (err) {
            displayError(err);
        }
    }

    onDeleteSize(index: number) {
        this.sizes.splice(index, 1);
    }

    onPadEdit(index: number) {
        this.modalPad = Object.assign({}, this.pads[index]);
        this.isPadModalActive = true;
        this.isPadModalInEditMode = true;
    }

    onPadAddAnother() {
        this.isPadModalActive = true;
        this.modalPad = {
            id: null,
            name: null!,
            category: null!,
            material: null!,
            texture: null!,
            color: null!,
            hasCenterHole: null!,
            image: null!,
            options: []
        };
    }

    onOptionAddAnother() {
        this.modalPad?.options.push({
            padSizeIndex: null,
            partNumbers: []
        });
    }

    onPadAddCancel() {
        this.modalPad = null;
        this.isPadModalActive = false;
    }

    async onPadAddDone() {
        const valid = await (this.$refs.modalValidator as any).validate();

        if (!valid) {
            return;
        }

        // Editted existing pad
        if (this.isPadModalInEditMode) {
            this.isPadModalInEditMode = false;
            const old = this.pads.findIndex(p => this.modalPad?.id == p.id);
            this.pads.splice(old, 1);
            this.pads.push(this.modalPad!);
        } else {
            this.pads.push(this.modalPad!);
        }

        this.isPadModalActive = false;
        this.modalPad = null;
    }

    /**
     * Don't allow a size to be picked on an option if it's already used by another
     * option on the same color.
     */
    isSizeDisabled(size: PadSizeCreateOrUpdate, pad: PadCreateOrUpdate) {
        const sizesUsedAlready = pad.options.map(o => o.padSizeIndex!).map(i => this.sizes[i]);

        return !sizesUsedAlready.every(s => s != size);
    }

    getThumbnailUrl(id: string) {
        return `${process.env.VUE_APP_API_DOMAIN}/image/${id}/thumbnail`;
    }
}
</script>
