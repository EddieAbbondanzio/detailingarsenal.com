import { PadCreateOrUpdate } from './pad-create-or-update';

export interface PadSeriesCreateRequest {
    name: string;
    brandId: string;
    pads: PadCreateOrUpdate[];
}