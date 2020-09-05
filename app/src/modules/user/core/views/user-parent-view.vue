<template>
    <div>
        <navbar>
            <navbar-item :to="{name: 'pads'}">Pads</navbar-item>
        </navbar>
        <div class="app-content">
            <router-view></router-view>
        </div>
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';

/**
 * Parent view for usermodule.
 */
@Component
export default class UserParentView extends Vue {}
</script>