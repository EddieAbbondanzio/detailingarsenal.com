<template>
    <div class="is-flex is-flex-column is-flex-grow-1">
        <navbar>
            <b-navbar-item :to="{ name: 'pads' }">Pads</navbar-item>
        </navbar>
        <div class="app-content">
            <router-view></router-view>
        </div>

        <!-- <nav-footer>
            <nav-footer-item :to="{name: 'pads'}" icon="checkbox-blank-circle" text="Pads" />
        </nav-footer>-->
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';

/**
 * Parent view for admin module.
 */
@Component
export default class ProductCatalogParentView extends Vue {}
</script>