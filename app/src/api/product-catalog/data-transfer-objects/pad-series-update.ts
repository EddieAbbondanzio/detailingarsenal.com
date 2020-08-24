import { PadUpdate } from '@/api/product-catalog/data-transfer-objects/pad-update';

export type PadSeriesUpdate = { id: string; name: string; brandId: string; pads: PadUpdate[] };
