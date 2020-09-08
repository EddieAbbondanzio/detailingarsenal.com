<template>
    <div class="is-flex is-flex-column is-flex-grow-1">
        <navbar>
            <navbar-item :to="{name: 'schedulingPanel'}">Scheduling</navbar-item>
            <navbar-item :to="{name: 'productCatalogPanel'}">Product Catalog</navbar-item>
        </navbar>
        <div class="app-content">
            <router-view></router-view>
        </div>
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';

/**
 * Parent view for admin module.
 */
@Component
export default class AdminParentView extends Vue {}
</script>