import { PadTexture } from '../pad-texture';
import { PadMaterial } from '../pad-material';
import { PolisherType } from '../polisher-type';
import { PadColorCreate } from './pad-color-create';
import { PadSizeCreateOrUpdate } from './pad-size-create-or-update';

export interface PadSeriesCreateRequest {
    name: string;
    brandId: string;
    texture: PadTexture;
    material: PadMaterial;
    polisherTypes: PolisherType[];
    sizes: PadSizeCreateOrUpdate[];
    colors: PadColorCreate[];
}