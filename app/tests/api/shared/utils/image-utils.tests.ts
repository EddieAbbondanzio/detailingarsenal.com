import { imageUrl, thumbnailUrl } from '@/api/shared/utils/image-utils';
import { TimeUtils } from '@/core/utils/time-utils';

describe('ImageUtils', () => {
    describe('imageUrl()', () => {
        test('returns proper url for image id', () => {
            expect(imageUrl('id')).toEqual('https://localhost:5001/images/id');
        });
    });

    describe('thumbnailUrl()', () => {
        test('returns proper url for image id', () => {
            expect(thumbnailUrl('id')).toEqual('https://localhost:5001/images/id/thumbnail');
        });
    });
});
