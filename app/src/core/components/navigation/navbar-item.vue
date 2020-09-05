<template>
    <b-navbar-item :class="classes" exact tag="router-link" :to="to">
        <slot></slot>
    </b-navbar-item>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';

/**
 * Navbar link that allows quick travel to another page.
 */
@Component({ name: 'navbar-item' })
export default class NavbarItem extends Vue {
    get classes() {
        return {
            'is-flex-tablet': true,
            'is-hidden-mobile': !this.isMobile
        };
    }

    @Prop({ required: true })
    to!: Location;

    @Prop({ default: false })
    isMobile!: boolean;
}
</script>