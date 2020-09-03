<template>
    <div class="is-flex is-flex-column is-flex-grow-1 has-overflow-y-hidden">
        <product-catalog-navbar />

        <div class="app-content has-overflow-y-hidden is-flex is-flex-column is-flex-grow-1">
            <router-view></router-view>
        </div>
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import ProductCatalogNavbar from '@/modules/product-catalog/core/components/product-catalog-navbar.vue';

@Component({
    components: {
        ProductCatalogNavbar
    }
})
export default class ProductCatalog extends Vue {}
</script>