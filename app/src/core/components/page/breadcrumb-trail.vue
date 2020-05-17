<template>
    <nav class="breadcrumb" aria-label="breadcrumbs">
        <ul>
            <slot></slot>
        </ul>
    </nav>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';

@Component({
    name: 'breadcrumb-trail'
})
export default class BreadcrumbTrail extends Vue {}
</script>