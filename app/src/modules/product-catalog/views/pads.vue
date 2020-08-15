<template>
    <div class="is-flex is-flex-column is-flex-grow-1 has-overflow-y-hidden">
        <product-catalog-navbar />

        <div
            class="app-content has-overflow-y-hidden is-flex is-flex-column is-flex-grow-1"
        >{{ pads }}</div>
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import ProductCatalogNavbar from '@/modules/product-catalog/components/product-catalog-navbar.vue';
import padStore from '@/modules/product-catalog/store/pad-store';
import { api } from '@/api';
import { displayLoading } from '@/core';

@Component({
    components: {
        ProductCatalogNavbar
    }
})
export default class Pads extends Vue {
    get pads() {
        return padStore.pads;
    }

    @displayLoading
    async created() {
        await padStore.init();
    }
}
</script>