<template>
    <b-navbar class="app-navbar" type="is-primary" :mobile-burger="false">
        <template slot="brand">
            <b-navbar-item class="is-flex is-flex-row is-align-items-center" exact>
                <h1
                    class="is-size-5-mobile is-size-4-tablet has-font-family-pacifico"
                >Detailing Arsenal</h1>
            </b-navbar-item>
        </template>
    </b-navbar>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import UserWidget from '@/modules/scheduling/user/components/user-widget.vue';
import userStore from '@/modules/scheduling/user/store/user-store';

@Component({
    name: 'product-catalog-navbar',
    components: {
        UserWidget
    }
})
export default class ProductCatalogNavbar extends Vue {}
</script>
