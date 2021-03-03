import { PartNumber } from './part-number';

export interface PadOption {
    id: string | null;
    padSizeId: string;
    partNumbers: PartNumber[];
}
