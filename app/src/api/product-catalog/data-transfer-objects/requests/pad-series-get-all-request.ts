import { Paging } from '@/api/core/data-transfer-objects/paging';
import { Measurement } from '../measurement';
import { PadCategory } from '../pad-category';
import { PadColor } from '../pad-color';
import { PadMaterial } from '../pad-material';
import { PadTexture } from '../pad-texture';

export interface PadSeriesGetAllRequest {
    brands?: string[];
    series?: string[];
    categories?: PadCategory[];
    materials?: PadMaterial[];
    textures?: PadTexture[];
    colors?: PadColor[];
    centerHole?: boolean[];
    diameters?: Measurement[];
    paging: Paging;
}
