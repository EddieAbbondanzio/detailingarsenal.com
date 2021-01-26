import { PadTexture } from '../pad-texture';
import { PadMaterial } from '../pad-material';
import { PolisherType } from '../polisher-type';
import { PadColorCreateOrUpdate } from './pad-color-create-or-update';
import { PadSizeCreateOrUpdate } from './pad-size-create-or-update';

export interface PadSeriesCreateRequest {
    name: string;
    brandId: string;
    polisherTypes: PolisherType[];
    sizes: PadSizeCreateOrUpdate[];
    colors: PadColorCreateOrUpdate[];
}