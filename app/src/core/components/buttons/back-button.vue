<template>
    <b-button
        @click="to != null ? $router.push(to) : $router.go(-1)"
        icon-left="arrow-left"
        type="is-text"
        size="is-medium"
    />
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { RawLocation } from 'vue-router';

/**
 * On screen button to return the user to the previous visited page.
 */
@Component({
    name: 'back-button'
})
export default class BackButton extends Vue {
    @Prop()
    to!: RawLocation;
}
</script>