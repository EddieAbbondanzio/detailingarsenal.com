<template>
    <div class="is-hidden-desktop">
        <div class="app-footer box has-padding-all-1">
            <nav class="level is-mobile">
                <slot></slot>
            </nav>
        </div>
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { RawLocation } from 'vue-router';

@Component({ name: 'nav-footer' })
export default class NavFooter extends Vue {
    links: { icon: string; title: string; route: RawLocation; exact?: boolean }[] = [
        { icon: 'calendar', title: 'Calendar', route: { name: 'calendar' }, exact: true },
        { icon: 'contacts', title: 'Clients', route: { name: 'clients' }, exact: true },
        { icon: 'cogs', title: 'Settings', route: { name: 'settings' } }
    ];
}
</script>