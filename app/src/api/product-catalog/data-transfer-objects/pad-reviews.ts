import { Rating } from './rating';
import { Review } from './review';

export interface PadReviews {
    rating: Rating;
    values: Review[];
    stats: ReviewStarStat[];
}

export interface ReviewStarStat {
    stars: number;
    count: number;
    percentage: number;
}
