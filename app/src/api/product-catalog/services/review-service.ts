import { http } from '@/api/shared/http';
import { PadReviews, ReviewCreateRequest } from '..';
import { Review } from '../data-transfer-objects/review';

export class ReviewService {
    async getForPad(padId: string) {
        const res = await http.get(`product-catalog/pads/${padId}/reviews`);
        return res.data as PadReviews;
    }

    async create(request: ReviewCreateRequest) {
        const res = await http.post(`product-catalog/reviews`, request);

        return new Review(
            res.data.username,
            res.data.createdDate,
            res.data.stars,
            res.data.cut,
            res.data.finish,
            res.data.title,
            res.data.body
        );
    }
}

export const reviewService = new ReviewService();
