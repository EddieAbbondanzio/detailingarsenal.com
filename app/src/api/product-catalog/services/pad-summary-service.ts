import { PagedArray } from '@/api/core/data-transfer-objects/paged-array';
import { PadSummary } from '../data-transfer-objects/pad-summary';

export class PadSummaryService {
    async getAll(): Promise<PagedArray<PadSummary>> {
        throw new Error();
    }
}
