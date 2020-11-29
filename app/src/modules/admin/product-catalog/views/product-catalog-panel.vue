<template>
    <page background="is-light">
        <template v-slot:header>
            <page-header
                title="Product catalog panel"
                description="Product catalog specific controls"
                icon="cogs"
                :backButton="false"
            >
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="Admin panel" :to="{ name: 'adminPanel' }" />
                        <breadcrumb name="Product catalog panel" :to="{ name: 'productCatalogPanel' }" :active="true" />
                    </breadcrumb-trail>
                </template>
            </page-header>
        </template>

        <list>
            <list-item
                v-for="setting in entries"
                :key="setting.title"
                :title="setting.title"
                :description="setting.description"
                :to="setting.to"
            >
                <template v-slot:icon>
                    <b-icon :icon="setting.icon" size="is-medium" type="is-primary" />
                </template>
            </list-item>
        </list>
    </page>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { RawLocation } from 'vue-router';

@Component({
    name: 'admin-panel',
})
export default class ProductCatalogPanel extends Vue {
    entries: { title: string; description: string; icon?: string; to: RawLocation }[] = [
        {
            title: 'Brands',
            description: 'Product brands',
            icon: 'watermark',
            to: { name: 'brands' },
        },
        {
            title: 'Pads',
            description: 'Pad series by manufacturers',
            icon: 'checkbox-blank-circle',
            to: { name: 'padSeries' },
        },
    ];
}
</script>