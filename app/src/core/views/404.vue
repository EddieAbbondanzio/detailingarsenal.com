<template>
    <div class="is-flex is-flex-column has-h-100 is-align-items-center is-justify-content-center">
        404. We'll this is embarassing!
        <a href="https://www.youtube.com/watch?v=dQw4w9WgXcQ&autoplay=1">Click to fix</a>
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';

@Component({})
export default class FourOhFour extends Vue {
    created() {
        // on 404 redirect to pads
        this.$router.push({ name: 'pads' });
    }
}
</script>