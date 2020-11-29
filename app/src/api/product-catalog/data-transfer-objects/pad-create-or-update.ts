import { PadCategory } from '@/api';
import { PadMaterial } from './pad-material';
import { PadTexture } from './pad-texture';
import { PolisherType } from './polisher-type';
import { Image } from './image';

export interface PadCreateOrUpdate {
    name: string,
    category: PadCategory,
    material: PadMaterial,
    texture: PadTexture,
    polisherTypes: PolisherType[],
    image: Image
}