import { PartNumber } from '../part-number';

export interface PadOptionCreateOrUpdate {
    padSizeIndex: number | null; // When we create options, pad sizes don't have ids yet.
    partNumbers: PartNumber[];
}
