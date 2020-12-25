import { PadMaterial } from '../pad-material';
import { PadTexture } from '../pad-texture';
import { PolisherType } from '../polisher-type';
import { PadColorUpdate } from './pad-color-update';
import { PadSizeCreateOrUpdate } from './pad-size-create-or-update';

export interface PadSeriesUpdateRequest {
    id: string;
    name: string;
    brandId: string;
    texture: PadTexture;
    material: PadMaterial;
    polisherTypes: PolisherType[];
    sizes: PadSizeCreateOrUpdate[];
    colors: PadColorUpdate[];
}