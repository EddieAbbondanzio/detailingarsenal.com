import { PadCreateOrUpdate } from '@/api/product-catalog/pad-series/data-transfer-objects/requests/pad-create-or-update';

export type PadSeriesUpdateRequest = { id: string; name: string; brandId: string; pads: PadCreateOrUpdate[] };
