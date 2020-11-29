<template>
    <page>
        <template v-slot:header>
            <page-header
                title="Brands"
                description="Need I say more"
                icon="watermark"
                :backButtonTo="{ name: 'productCatalogPanel' }"
            >
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="Admin panel" :to="{ name: 'adminPanel' }" />
                        <breadcrumb name="Product catalog panel" :to="{ name: 'productCatalogPanel' }" />
                        <breadcrumb name="Brands" :to="{ name: 'brands' }" :active="true" />
                    </breadcrumb-trail>
                </template>

                <template v-slot:action>
                    <create-button :to="{ name: 'createBrand' }" text="Create brand" />
                </template>
            </page-header>
        </template>

        <list>
            <list-item v-for="b in brands" :key="b.id" :title="b.name">
                <template v-slot:actions>
                    <edit-delete-dropdown @edit="onEdit(b)" @delete="onDelete(b)" />
                </template>
            </list-item>
        </list>
    </page>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { displayLoading, confirmDelete, toast, displayError } from '@/core';
import brandStore from '@/modules/admin/product-catalog/store/brand-store';
import { Brand } from '@/api';

@Component
export default class Brands extends Vue {
    get brands() {
        return brandStore.brands;
    }

    @displayLoading
    async created() {
        await brandStore.init();
    }

    async onEdit(brand: Brand) {
        this.$router.push({
            name: 'editBrand',
            params: {
                id: brand.id,
            },
        });
    }

    async onDelete(brand: Brand) {
        const del = await confirmDelete('brand', brand.name);

        if (del) {
            try {
                await brandStore.delete(brand);
                toast(`Deleted brand ${brand.name}`);
            } catch (err) {
                displayError(err);
            }
        }
    }
}
</script>