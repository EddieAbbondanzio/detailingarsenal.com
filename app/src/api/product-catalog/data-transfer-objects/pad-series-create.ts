import { PadCreate } from '@/api/product-catalog/data-transfer-objects/pad-create';

export type PadSeriesCreate = { name: string; brandId: string; pads: PadCreate[] };
