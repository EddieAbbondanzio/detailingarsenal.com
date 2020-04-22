<template>
    <div>
        landing
        <b-timepicker v-model="test" @input="onInput" />
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';

@Component({
    name: 'landing'
})
export default class Landing extends Vue {
    test: Date = new Date('04/20/1969');

    onInput() {
        console.log(this.test);
    }
}
</script>