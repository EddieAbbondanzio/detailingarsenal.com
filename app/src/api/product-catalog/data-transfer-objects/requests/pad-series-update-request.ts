import { PadSize } from '../pad-size';
import { PadCreateOrUpdate } from '../pad-create-or-update';

export interface PadSeriesUpdateRequest {
    id: string;
    name: string;
    brandId: string;
    sizes: PadSize[];
    pads: PadCreateOrUpdate[];
}