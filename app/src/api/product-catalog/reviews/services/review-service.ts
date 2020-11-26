import { http } from '@/api/core/http';
import { Review } from '../data-transfer-objects/review';
import { ReviewCreateRequest } from '../data-transfer-objects/review-create-request';

export class ReviewService {
    async getForPad(padId: string) {
        const res = await http.get(`product-catalog/review/pad/${padId}`);
        return (res.data as any[]).map(d => new Review(
            d.username, d.createdDate, d.stars, d.cut, d.finish, d.title, d.body
        ));
    }

    async create(request: ReviewCreateRequest) {
        const res = await http.post(`product-catalog/review`, request);

        return new Review(
            res.data.username, res.data.createdDate, res.data.stars, res.data.cut, res.data.finish, res.data.title, res.data.body
        )
    }
}