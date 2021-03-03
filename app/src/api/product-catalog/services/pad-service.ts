import { Pad } from '@/api/admin/data-transfer-objects/pad';
import { PagedArray } from '@/api/core';

export class PadSummaryService {
    async getAll(): Promise<PagedArray<Pad>> {
        throw new Error();
    }
}
