import { Measurement } from "../measurement";

export interface PadSizeCreateOrUpdate {
    diameter: Measurement,
    thickness?: Measurement,
}