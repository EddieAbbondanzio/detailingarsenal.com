<template>
    <page>
        <template v-slot:header>
            <page-header
                title="Pad Series"
                description="Pad series by manufacturer"
                icon="checkbox-blank-circle"
                :backButtonTo="{ name: 'productCatalogPanel' }"
            >
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="Admin panel" :to="{ name: 'adminPanel' }" />
                        <breadcrumb name="Product catalog panel" :to="{ name: 'productCatalogPanel' }" />
                        <breadcrumb name="Pad series" :to="{ name: 'padSeries' }" :active="true" />
                    </breadcrumb-trail>
                </template>

                <template v-slot:action>
                    <create-button :to="{ name: 'createPadSeries' }" text="Create pad series" />
                </template>
            </page-header>
        </template>

        <b-table
            :data="series"
            paginated
            backend-pagination
            :total="paging.total"
            :per-page="paging.pageSize"
            @page-change="onPageChange"
        >
            <b-table-column label="Name" field="brand" sortable v-slot="props" cell-class="has-vertical-align-middle">
                <router-link
                    class="is-inline-block has-text-weight-bold has-text-black"
                    :to="{ name: 'padSeriesDetails', params: { id: props.row.id } }"
                    >{{ props.row.brand.name + ' ' + props.row.name }}</router-link
                >
            </b-table-column>
            <b-table-column label="Sizes" sortable v-slot="props" cell-class="has-vertical-align-middle" numeric>
                {{ props.row.sizes.length }}
            </b-table-column>
            <b-table-column label="Pads" sortable v-slot="props" cell-class="has-vertical-align-middle" numeric>
                {{ props.row.pads.length }}
            </b-table-column>
            <b-table-column v-slot="props" cell-class="has-vertical-align-middle">
                <update-delete-dropdown @update="onUpdate(props.row)" @delete="onDelete(props.row)" />
            </b-table-column>
        </b-table>
    </page>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { displayLoading, confirmDelete, toast, displayError } from '@/core';
import { Pad, PadSeries } from '@/api/admin';
import adminPadStore from '../../store/admin-pad-store';

@Component
export default class Pads extends Vue {
    get series() {
        return adminPadStore.series;
    }

    get paging() {
        return adminPadStore.paging;
    }

    @displayLoading
    async created() {
        await adminPadStore.init();
    }

    async onUpdate(pad: PadSeries) {
        this.$router.push({
            name: 'updatePadSeries',
            params: {
                id: pad.id
            }
        });
    }

    async onDelete(pad: PadSeries) {
        const del = await confirmDelete('pad series', pad.name);

        if (del) {
            try {
                await adminPadStore.delete(pad);
                toast(`Deleted pad series ${pad.name}`);
            } catch (err) {
                displayError(err);
            }
        }
    }

    @displayLoading
    async onPageChange(pageNumber: number) {
        await adminPadStore.goToPage(pageNumber);
    }
}
</script>
