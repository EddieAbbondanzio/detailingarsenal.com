import { Image } from '../image';
import { PadCategory } from '../pad-category';
import { PadColor } from '../pad-color';
import { PadMaterial } from '../pad-material';
import { PadOption } from '../pad-option';
import { PadTexture } from '../pad-texture';
import { PadOptionCreateOrUpdate } from './pad-option-create-or-update';

export interface PadCreateOrUpdate {
    id: string | null;
    name: string;
    category: PadCategory[];
    material: PadMaterial | null;
    texture: PadTexture | null;
    color: PadColor | null;
    hasCenterHole: boolean | null;
    image: Image | string | null;
    options: PadOptionCreateOrUpdate[];
}
