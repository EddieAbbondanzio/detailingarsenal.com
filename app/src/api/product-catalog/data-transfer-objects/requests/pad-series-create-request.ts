import { PadSeriesSize } from '../pad-series-size';
import { PadCreateOrUpdate } from '../pad-create-or-update';

export interface PadSeriesCreateRequest {
    name: string;
    brandId: string;
    sizes: PadSeriesSize[];
    pads: PadCreateOrUpdate[];
}