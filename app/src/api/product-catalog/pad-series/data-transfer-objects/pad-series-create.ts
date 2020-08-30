import { PadCreate } from '@/api/product-catalog/pad-series/data-transfer-objects/pad-create';

export type PadSeriesCreate = { name: string; brandId: string; pads: PadCreate[] };
