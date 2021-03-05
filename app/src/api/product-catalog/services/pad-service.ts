import { Pad } from '@/api/admin/data-transfer-objects/pad';
import { PagedArray } from '@/api/shared';

export class PadService {
    async getAll(req: { paging: any } | null = null): Promise<PagedArray<Pad>> {
        throw new Error();
    }

    async get(id: string): Promise<Pad> {
        throw new Error();
    }
}

export const padService = new PadService();
