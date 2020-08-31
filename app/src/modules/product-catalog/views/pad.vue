<template>
    <div class="has-background-danger">FUCK OFF</div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';

@Component
export default class PadView extends Vue {
    created() {
        console.log('hup');
    }
    // size is a query string param.
    // Lets us mock the page to look like specifically that size
}
</script>