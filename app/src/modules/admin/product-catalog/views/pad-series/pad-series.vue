<template>
    <page>
        <template v-slot:header>
            <page-header
                title="Pads"
                description="Pad series by manufacturer"
                icon="checkbox-blank-circle"
                :backButtonTo="{ name: 'productCatalogPanel' }"
            >
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="Admin Panel" :to="{ name: 'adminPanel' }" />
                        <breadcrumb name="Product Catalog Panel" :to="{ name: 'productCatalogPanel' }" />
                        <breadcrumb name="Pads" :to="{ name: 'pads' }" :active="true" />
                    </breadcrumb-trail>
                </template>

                <template v-slot:action>
                    <create-button :to="{ name: 'createPadSeries' }" text="Create pad" />
                </template>
            </page-header>
        </template>

        <list>
            <list-item
                v-for="s in series"
                :key="s.id"
                :title="s.name"
                :description="s.brand.name"
                :to="{ name: 'padSeriesDetails', params: { id: s.id } }"
            >
                <template v-slot:actions>
                    <edit-delete-dropdown @edit="onEdit(s)" @delete="onDelete(s)" />
                </template>
            </list-item>
        </list>
    </page>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { displayLoading, confirmDelete, toast, displayError } from '@/core';
import { Pad, PadSeries } from '@/api';
import adminPadStrore from '../../store/admin-pad-strore';

@Component
export default class Pads extends Vue {
    // get series() {
    //     return padSeriesStore.series;
    // }

    @displayLoading
    async created() {
        await adminPadStrore.init();
    }

    async onEdit(pad: Pad) {
        this.$router.push({
            name: 'editPad',
            params: {
                id: pad.id
            }
        });
    }

    async onDelete(pad: PadSeries) {
        const del = await confirmDelete('pad', pad.name);

        if (del) {
            try {
                // await padSeriesStore.delete(pad);
                toast(`Deleted pad ${pad.name}`);
            } catch (err) {
                displayError(err);
            }
        }
    }
}
</script>