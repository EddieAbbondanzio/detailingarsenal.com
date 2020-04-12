<template>
    <div class="is-hidden-desktop">
        <div class="app-footer box has-padding-all-1">
            <nav class="level is-mobile">
                <div class="level-item" v-for="link in links" :key="link.title">
                    <router-link
                        :exact="link.exact"
                        class="is-flex is-flex-column is-align-items-center has-text-dark"
                        :to="link.route"
                    >
                        <b-icon style :icon="link.icon" />
                        <span class="is-size-7">{{ link.title }}</span>
                    </router-link>
                </div>
            </nav>
        </div>
    </div>
</template>

<style lang="sass" scoped>
.app-footer
    position: absolute
    left: 0px
    right: 0px
    bottom: 0px

    a.is-active
        color: $primary!important
</style>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { RawLocation } from 'vue-router';

/**
 * Mobile only nav footer that supplements the burger
 */
@Component({
    name: 'app-footer'
})
export default class AppFooter extends Vue {
    links: { icon: string; title: string; route: RawLocation; exact?: boolean }[] = [
        { icon: 'calendar', title: 'Calendar', route: { name: 'calendar' }, exact: true },
        { icon: 'contacts', title: 'Clients', route: { name: 'clients' }, exact: true },
        { icon: 'cogs', title: 'Settings', route: { name: 'settings' } }
    ];
}
</script>
