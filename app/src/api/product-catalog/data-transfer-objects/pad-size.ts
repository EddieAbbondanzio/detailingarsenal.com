import { Measurement } from '@/api/shared';

export interface PadSize {
    diameter: Measurement;
    thickness?: Measurement;
}
