import { Image } from '../image';
import { PadColor } from '../pad-color';
import { PadMaterial } from '../pad-material';
import { PadOption } from '../pad-option';
import { PadTexture } from '../pad-texture';
import { PadOptionCreateOrUpdate } from './pad-option-create-or-update';

export interface PadCreateOrUpdate {
    id: string | null;
    name: string;
    category: string;
    material: PadMaterial | null;
    texture: PadTexture | null;
    color: PadColor | null;
    image: Image | string | null;
    options: PadOptionCreateOrUpdate[];
}
