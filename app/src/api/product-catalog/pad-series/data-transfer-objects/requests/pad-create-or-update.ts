import { PadCategory } from '@/api';
import { Image } from '@/api/product-catalog/common/data-transfer-objects/image';
import { PadMaterial } from '../pad-material';
import { PadSize } from '../pad-size';
import { PadTexture } from '../pad-texture';
import { PolisherType } from '../polisher-type';

export interface PadCreateOrUpdate {
    name: string,
    category: PadCategory,
    material: PadMaterial,
    texture: PadTexture,
    sizes: PadSize[],
    polisherTypes: PolisherType[],
    image: Image
}