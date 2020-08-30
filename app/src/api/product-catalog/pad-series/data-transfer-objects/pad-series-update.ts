import { PadUpdate } from '@/api/product-catalog/pad-series/data-transfer-objects/pad-update';

export type PadSeriesUpdate = { id: string; name: string; brandId: string; pads: PadUpdate[] };
