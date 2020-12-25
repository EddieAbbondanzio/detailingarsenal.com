<template>
    <page>
        <template v-slot:header>
            <page-header :title="value != null ? value.name : ``">
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
                <p class="is-size-4 subtitle">{{ value.brand.name }}</p>

                <p><span class="has-text-weight-bold">Material: </span>{{ value.material }}</p>
                <p><span class="has-text-weight-bold">Texture: </span>{{ value.texture }}</p>
                <p>
                    <span class="has-text-weight-bold">Polisher types: </span
                    ><span v-for="pt in value.polisherTypes" :key="pt">{{ pt }}</span>
                    <span v-if="!value.polisherTypes">None specified</span>
                </p>
            </div>

            <input-group-header text="Colors" />

            <b-table :data="value.colors">
                <b-table-column v-slot="props" label="Name" field="label" sortable>{{ props.row.name }}</b-table-column>
                <b-table-column v-slot="props" label="Category" field="action" sortable>{{
                    props.row.category
                }}</b-table-column>
                <b-table-column v-slot="props" label="Image" field="scope" sortable>{{
                    props.row.image != null ? props.row.image.name : ''
                }}</b-table-column>

                <template slot="empty">
                    <div class="is-flex is-justify-content-center">There's nothing here!</div>
                </template>
            </b-table>

            <input-group-header text="Sizes" />

            <b-table :data="value.sizes">
                <b-table-column v-slot="props" label="Diameter" field="diameter" sortable>{{
                    props.row.diameter
                }}</b-table-column>
                <b-table-column v-slot="props" label="Thickness" field="thickness" sortable
                    >{{ props.row.thickness }}
                </b-table-column>

                <template slot="empty">
                    <div class="is-flex is-justify-content-center">There's nothing here!</div>
                </template>
            </b-table>
        </div>
    </page>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import appStore from '@/core/store/app-store';
import { PadSize, Permission, Role } from '@/api';
import { displayLoading } from '@/core';
import adminPadStore from '../../store/admin-pad-store';

@Component({
    name: 'role',
})
export default class PadSeriesDetails extends Vue {
    get value() {
        return adminPadStore.series.find((s) => s.id == this.$route.params.id);
    }

    get sizes() {
        return [new PadSize(1, 1)];
    }

    @displayLoading
    async created() {
        await adminPadStore.init();
    }
}
</script>