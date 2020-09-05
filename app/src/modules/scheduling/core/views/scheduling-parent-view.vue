<template>
    <div>
        <navbar>
            <navbar-item :to="{ name: 'calendar' }">Calendar</navbar-item>
            <navbar-item :to="{ name: 'clients' }">Clients</navbar-item>
            <navbar-item :to="{ name: 'settings' }">Settings</navbar-item>
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
export default class SchedulingParentView extends Vue {}
</script>