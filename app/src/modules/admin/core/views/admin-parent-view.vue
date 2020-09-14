<template>
    <div class="is-flex is-flex-column is-flex-grow-1">
        <navbar>
            <navbar-item :to="{name: 'schedulingPanel'}">Scheduling</navbar-item>
            <navbar-item :to="{name: 'productCatalogPanel'}">Product Catalog</navbar-item>
        </navbar>
        <div class="app-content">
            <router-view></router-view>
        </div>

        <nav-footer>
            <nav-footer-item :to="{name: 'schedulingPanel'}" text="Scheduling" icon="calendar" />
            <nav-footer-item
                :to="{name: 'productCatalogPanel'}"
                text="Product Catalog"
                icon="book-open"
            />
        </nav-footer>
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