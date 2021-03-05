import { Measurement } from '@/api/shared';

export interface PadSizeCreateOrUpdate {
    id: string | null;
    diameter: Measurement;
    thickness: Measurement | null;
}
