import { Measurement } from '../measurement';

export interface PadSizeCreateOrUpdate {
    id: string | null;
    diameter: Measurement;
    thickness: Measurement | null;
}
