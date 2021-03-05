import { PolisherType } from '../../../shared/data-transfer-objects/polisher-type';
import { PadCreateOrUpdate } from './pad-create-or-update';
import { PadSizeCreateOrUpdate } from './pad-size-create-or-update';

export interface PadSeriesCreateRequest {
    name: string;
    brandId: string;
    polisherTypes: PolisherType[];
    sizes: PadSizeCreateOrUpdate[];
    pads: PadCreateOrUpdate[];
}
