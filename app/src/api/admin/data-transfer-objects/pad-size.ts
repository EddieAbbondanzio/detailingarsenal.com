import { Measurement } from '@/api/shared/data-transfer-objects/measurement';
import { MeasurementUnit } from '../../shared/data-transfer-objects/measurement-unit';

export class PadSize {
    constructor(public id: string, public diameter: Measurement, public thickness?: Measurement) {}

    static isThin(p: PadSize) {
        if (p.thickness == null) {
            return false;
        }

        return (
            (p.thickness.amount < 0.75 && p.thickness.unit == MeasurementUnit.Inches) ||
            (p.thickness?.amount < 19.05 && p.thickness?.unit == MeasurementUnit.Millimeters)
        );
    }
}
