import { PadCreateRequest } from '@/api/product-catalog/pad-series/data-transfer-objects/requests/pad-create-request';

export type PadSeriesCreateRequest = { name: string; brandId: string; pads: PadCreateRequest[] };
