<template>
    <div>
        <b-navbar class="app-navbar" type="is-primary" :mobile-burger="true">
            <template slot="brand">
                <b-navbar-item
                    class="is-flex is-flex-row is-align-items-center"
                    exact
                    tag="a"
                    :to="brandTo"
                >
                    <h1
                        class="is-size-5-mobile is-size-4-tablet has-font-family-pacifico"
                    >Detailing Arsenal</h1>
                </b-navbar-item>
            </template>

            <!-- Only shows on desktop and bigger -->
            <template slot="start">
                <slot></slot>
            </template>

            <template slot="end">
                <b-navbar-item tag="div">
                    <user-widget />
                </b-navbar-item>
            </template>

            <!-- Hack.  -->
            <template slot="burger">
                <user-widget
                    class="is-hidden-desktop is-align-self-center"
                    style="margin-left: auto;"
                />
            </template>
        </b-navbar>
    </div>
</template>

<style lang="sass" scoped>
// friggen hack. Needed to show user-widget on mobile too
.app-navbar
    .navbar-brand
        .navbar-item
            background: $primary!important

    .navbar-menu
        display: flex!important
        background: $primary!important
        padding: 0px!important
</style>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import UserWidget from '@/modules/user/core/components/user-widget.vue';

@Component({
    name: 'navbar',
    components: {
        UserWidget
    }
})
export default class Navbar extends Vue {
    @Prop({ default: '/' })
    brandTo!: Location;
}
</script>
