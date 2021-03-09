<template>
    <div style="max-width: 300px;">
        <div class="has-margin-bottom-3">
            <div class="is-flex is-flex-row is-align-items-center">
                <stars v-model="value.stars" :readOnly="true" :hideCount="true" />
                <p class="has-margin-left-2 is-size-5">{{ value.stars }} / 5</p>
            </div>
            <p class="has-text-grey">{{ value.reviewCount }} {{ value.reviewCount == 1 ? 'review' : 'reviews' }}</p>
        </div>

        <div>
            <div
                class="is-flex is-flex-row is-align-items-center is-justify-content-space-between"
                v-for="s in stats"
                :key="s.stars"
            >
                <p class="has-margin-right-1">{{ s.stars }} star</p>
                <b-progress
                    :title="`${s.count} ${s.stars} star ${s.count == 1 ? 'review' : 'reviews'}`"
                    class="has-margin-all-0"
                    :value="s.percentage"
                    :max="1"
                    type="is-warning"
                />
                <p class="has-text-right" style="width: 64px;">{{ s.percentage * 100 }}%</p>
            </div>
        </div>
    </div>
</template>
<script lang="ts">
import Vue from 'vue';
import Component from 'vue-class-component';
import Stars from '@/modules/product-catalog/core/components/stars.vue';
import { PadReviews, Rating, ReviewStarStat } from '@/api/product-catalog';
import { Prop } from 'vue-property-decorator';

@Component({
    components: {
        Stars
    }
})
export default class RatingStats extends Vue {
    get stats() {
        const stats: ReviewStarStat[] = [];

        for (let i = 5; i > 0; i--) {
            const actual = this.value.stats?.find(s => s.stars == i);
            stats.push(actual ?? { stars: i, count: 0, percentage: 0 });
        }

        return stats;
    }

    @Prop()
    value!: PadReviews;
}
</script>
