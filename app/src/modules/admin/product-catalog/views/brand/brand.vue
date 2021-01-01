<template>
    <page>
        <template v-slot:header>
            <page-header :title="name" icon="watermark" :backButtonTo="{ name: 'brands' }">
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="Admin panel" :to="{ name: 'adminPanel' }" />
                        <breadcrumb name="Product catalog panel" :to="{ name: 'productCatalogPanel' }" />
                        <breadcrumb name="Brands" :to="{ name: 'brands' }" />
                        <breadcrumb :name="name" :to="$route" :active="true" />
                    </breadcrumb-trail>
                </template>

                <template v-slot:action>
                    <update-button :to="{ name: 'updateBrand' }" text="Update" />
                </template>
            </page-header>
        </template>

        <div>
            {{ name }}
        </div>
    </page>
</template>

<script lang="ts">
import { Brand } from '@/api';
import { displayLoading } from '@/core';
import Vue from 'vue';
import Component from 'vue-class-component';
import brandStore from '../../store/brand-store';

@Component
export default class BrandView extends Vue {
    name: string = '';

    @displayLoading
    async created() {
        await brandStore.init();
        this.name = brandStore.brands.find((b) => b.id == this.$route.params.id)!.name;
    }
}
</script>