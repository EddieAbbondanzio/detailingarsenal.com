import { PartNumber } from '../part-number';

export interface PadOptionCreateOrUpdate {
    id: string | null;
    padSizeIndex: number | null; // When we create options, pad sizes don't have ids yet.
    partNumbers: PartNumber[];
}
