<template>
    <div style="max-width: 300px;">
        <div class="has-margin-bottom-3">
            <div class="is-flex is-flex-row is-align-items-center">
                <stars v-model="value.stars" :readOnly="true" />
                <p class="has-margin-left-2 is-size-5">{{ value.stars }} / 5</p>
            </div>
            <p class="has-text-grey">{{ value.reviewCount }} {{ value.reviewCount == 1 ? 'review' : 'reviews' }}</p>
        </div>

        <div>
            <div
                class="is-flex is-flex-row is-align-items-center is-justify-content-space-between"
                v-for="s in value.stats"
                :key="s.stars"
            >
                <p class="has-margin-right-1">{{ s.star }} star</p>
                <b-progress
                    :title="`${s.count} star reviews`"
                    class="has-margin-all-0"
                    :value="s.percentage"
                    :max="1"
                    type="is-warning"
                />
                <p>{{ s.percentage }}%</p>
            </div>
        </div>
    </div>
</template>
<script lang="ts">
import Vue from 'vue';
import Component from 'vue-class-component';
import Stars from '@/modules/product-catalog/core/components/stars.vue';
import { Rating, RatingStarStat } from '@/api';
import { Prop } from 'vue-property-decorator';

@Component({
    components: {
        Stars
    }
})
export default class RatingStats extends Vue {
    @Prop()
    value!: Rating;

    created() {
        console.log(this.value.stats);
    }
}
</script>
