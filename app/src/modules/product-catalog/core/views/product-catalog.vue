<template>
    <div class="is-flex is-flex-column is-flex-grow-1 has-overflow-y-hidden">
        <b-navbar class="app-navbar" type="is-primary" :mobile-burger="false">
            <template slot="brand">
                <b-navbar-item class="is-flex is-flex-row is-align-items-center" exact>
                    <h1
                        class="is-size-5-mobile is-size-4-tablet has-font-family-pacifico"
                    >Detailing Arsenal</h1>
                </b-navbar-item>
            </template>
        </b-navbar>

        <div class="app-content has-overflow-y-hidden is-flex is-flex-column is-flex-grow-1">
            <router-view></router-view>
        </div>
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';

@Component
export default class ProductCatalog extends Vue {}
</script>