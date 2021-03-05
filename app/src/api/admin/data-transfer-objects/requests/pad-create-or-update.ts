import { Image } from '../../../shared/data-transfer-objects/image';
import { PadColor } from '../../../shared/data-transfer-objects/pad-color';
import { PadOption } from '../pad-option';
import { PadTexture } from '../../../shared/data-transfer-objects/pad-texture';
import { PadOptionCreateOrUpdate } from './pad-option-create-or-update';
import { PadCategory, PadMaterial } from '@/api/shared';

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
