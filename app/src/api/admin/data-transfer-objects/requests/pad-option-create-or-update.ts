import { PartNumber } from '../../../shared/data-transfer-objects/part-number';
import { PartNumberCreateOrUpdate } from './part-number-create-or-update';

export interface PadOptionCreateOrUpdate {
    id: string | null;
    padSizeIndex: number | null; // When we create options, pad sizes don't have ids yet.
    partNumbers: PartNumberCreateOrUpdate[];
}
