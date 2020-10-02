import { PadCreateOrUpdate } from './pad-create-or-update';

export interface PadSeriesUpdateRequest {
    id: string;
    name: string;
    brandId: string;
    pads: PadCreateOrUpdate[];
}