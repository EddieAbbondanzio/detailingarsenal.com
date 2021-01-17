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

        <list>
            <list-item
                v-for="s in series"
                :key="s.id"
                :title="s.name"
                :description="s.brand.name"
                :to="{ name: 'padSeriesDetails', params: { id: s.id } }"
            >
                <template v-slot:actions>
                    <update-delete-dropdown @update="onUpdate(s)" @delete="onDelete(s)" />
                </template>
            </list-item>
        </list>
    </page>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { displayLoading, confirmDelete, toast, displayError } from '@/core';
import { PadColor, PadSeries } from '@/api';
import adminPadStore from '../../store/admin-pad-store';

@Component
export default class Pads extends Vue {
    get series() {
        return adminPadStore.series;
    }

    @displayLoading
    async created() {
        await adminPadStore.init();
    }

    async onUpdate(pad: PadSeries) {
        this.$router.push({
            name: 'updatePadSeries',
            params: {
                id: pad.id,
            },
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
}
</script>