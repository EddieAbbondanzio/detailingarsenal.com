<template>
    <page>
        <template v-slot:header>
            <page-header :title="value != null ? value.name : ``">
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="Admin panel" :to="{name: 'adminPanel'}" />
                        <breadcrumb
                            name="Product catalog panel"
                            :to="{name: 'productCatalogPanel'}"
                        />
                        <breadcrumb name="Pad series" :to="{name: 'padSeries'}" />
                        <breadcrumb
                            :name="value != null ? value.name : ''"
                            :to="{name: 'padSeriesDetails', params: $route.params}"
                            :active="true"
                        />
                    </breadcrumb-trail>
                </template>

                <template v-slot:action>
                    <edit-button :to="{name: 'editPadSeries', params: { id: $route.params.id }}" />
                </template>
            </page-header>
        </template>

        <div class="box is-shadowless" v-if="value != null">
            <div class="has-margin-bottom-4">
                <p class="is-size-4 title">{{ value.name }}</p>
                <p class="is-size-4 subtitle">{{ value.brand.name }}</p>
            </div>

            <input-group-header text="Pads" />

            <b-table :data="value.pads">
                <template slot-scope="props">
                    <b-table-column label="Name" field="label" sortable>{{ props.row.name }}</b-table-column>
                    <b-table-column
                        label="Category"
                        field="action"
                        sortable
                    >{{ props.row.category }}</b-table-column>
                    <b-table-column
                        label="Image"
                        field="scope"
                        sortable
                    >{{ props.row.image != null ? props.row.image.name : '' }}</b-table-column>
                </template>

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
import { Permission, Role } from '@/api';
import { displayLoading } from '@/core';
import padSeriesStore from '../../store/pad-series-store';

@Component({
    name: 'role'
})
export default class PadSeriesDetails extends Vue {
    // get value() {
    //     return padSeriesStore.series.find(s => s.id == this.$route.params.id);
    // }

    @displayLoading
    async created() {
        await padSeriesStore.init();
    }
}
</script>