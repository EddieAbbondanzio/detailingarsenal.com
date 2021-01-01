<template>
    <page>
        <template v-slot:header>
            <page-header :title="title" :description="description">
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="Admin panel" :to="{ name: 'adminPanel' }" />
                        <breadcrumb name="Product catalog panel" :to="{ name: 'productCatalogPanel' }" />
                        <breadcrumb name="Pad series" :to="{ name: 'padSeries' }" />
                        <breadcrumb
                            :name="value != null ? value.name : ''"
                            :to="{ name: 'padSeriesDetails', params: $route.params }"
                            :active="true"
                        />
                    </breadcrumb-trail>
                </template>

                <template v-slot:action>
                    <edit-button :to="{ name: 'editPadSeries', params: { id: $route.params.id } }" />
                </template>
            </page-header>
        </template>

        <div class="box is-shadowless" v-if="value != null">
            <div class="has-margin-bottom-4">
                <p class="is-size-4 title">{{ value.name }}</p>
                <p class="is-size-4 subtitle">By {{ value.brand.name }}</p>

                <p><span class="has-text-weight-bold">Material: </span>{{ value.material | uppercaseFirst }}</p>
                <p><span class="has-text-weight-bold">Texture: </span>{{ value.texture | uppercaseFirst }}</p>

                <div class="has-margin-bottom-3">
                    <p class="is-size-6 has-text-weight-bold has-margin-bottom-1">Recommended For Polisher Type(s)</p>
                    <b-taglist>
                        <polisher-type-tag v-for="t in value.polisherTypes" :key="t" :value="t" />
                    </b-taglist>
                </div>
            </div>

            <b-field label="Sizes">
                <b-table :data="value.sizes">
                    <b-table-column v-slot="props" label="Diameter" field="diameter" sortable>{{
                        props.row.diameter | measurement
                    }}</b-table-column>
                    <b-table-column v-slot="props" label="Thickness" field="thickness" sortable
                        >{{ props.row.thickness | measurement }}
                    </b-table-column>

                    <template slot="empty">
                        <div class="is-flex is-justify-content-center">There's nothing here!</div>
                    </template>
                </b-table>
            </b-field>

            <b-field label="Colors">
                <b-table :data="value.colors" detailed>
                    <b-table-column v-slot="props" label="Name" field="label" sortable>{{
                        props.row.name
                    }}</b-table-column>
                    <b-table-column v-slot="props" label="Category" field="action" sortable>{{
                        props.row.category | uppercaseFirst
                    }}</b-table-column>
                    <b-table-column v-slot="props" label="Image" field="scope" sortable>
                        <a :href="props.row.imageUrl" target="_blank">
                            <img :src="props.row.thumbnailUrl" style="max-height: 40px !important" />
                        </a>
                    </b-table-column>

                    <template slot="empty">
                        <div class="is-flex is-justify-content-center">There's nothing here!</div>
                    </template>

                    <template v-slot:detail="props">
                        <b-field label="Options">
                            <b-table :data="props.row.options">
                                <b-table-column v-slot="props" label="Pad size">
                                    {{ getPadSize(props.row.padSizeId) }}
                                </b-table-column>
                                <b-table-column v-slot="props" label="Part number">
                                    {{ props.row.partNumber }}
                                </b-table-column>
                            </b-table>
                        </b-field>
                    </template>
                </b-table>
            </b-field>
        </div>
    </page>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import appStore from '@/core/store/app-store';
import { PadSize, Permission, Role } from '@/api';
import { displayLoading } from '@/core';
import adminPadStore from '../../store/admin-pad-store';
import PolisherTypeTag from '@/modules/shared/components/polisher-type-tag.vue';
import { measurement } from '@/modules/shared/filters/measurement';

@Component({
    name: 'role',
    components: {
        PolisherTypeTag,
    },
    filters: {
        measurement,
    },
})
export default class PadSeriesDetails extends Vue {
    get value() {
        return adminPadStore.series.find((s) => s.id == this.$route.params.id);
    }

    get title() {
        return this.value?.name;
    }

    get description() {
        return `By ${this.value?.brand.name}`;
    }

    @displayLoading
    async created() {
        await adminPadStore.init();
    }

    getPadSize(id: string) {
        const s = this.value?.sizes.find((s) => s.id == id)!;

        return `${s.diameter.amount}${s.diameter.unit}`;
    }
}
</script>