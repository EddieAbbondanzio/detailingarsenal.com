import { Measurement } from "./measurement";
import { MeasurementUnit } from "./measurement-unit";

export interface PadSize {
    id: string;
    diameter: Measurement;
    thickness?: Measurement;
}