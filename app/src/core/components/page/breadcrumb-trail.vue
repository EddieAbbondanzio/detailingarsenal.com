<template>
    <nav class="breadcrumb has-margin-bottom-2" aria-label="breadcrumbs">
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