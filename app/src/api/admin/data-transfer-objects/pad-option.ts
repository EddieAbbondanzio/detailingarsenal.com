import { PartNumber } from '../../shared/data-transfer-objects/part-number';

export interface PadOption {
    id: string | null;
    padSizeId: string;
    partNumbers: PartNumber[];
}
