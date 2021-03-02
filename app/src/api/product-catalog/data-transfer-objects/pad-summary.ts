import { PadCategory } from './pad-category';
import { PadMaterial } from './pad-material';
import { PadTexture } from './pad-texture';
import { PolisherType } from './polisher-type';
import { Rating } from './rating';

export interface PadSummary {
    id: string;
    name: string;
    series: { id: string; name: string };
    brand: { id: string; name: string };
    category: PadCategory;
    material: PadMaterial;
    texture: PadTexture;
    cut: number | null;
    finish: number | null;
    rating: Rating;
    polisherTypes: PolisherType[];
    hasCenterHole: boolean;
}
