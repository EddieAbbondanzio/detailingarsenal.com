export function inchify(measurement: number) {
    if (measurement == null) {
        return '';
    }

    const length = measurement.toString().length - 2;
    let denominator = Math.pow(10, length);
    let numerator = measurement * denominator;

    const divisor = greatestCommonDenominator(numerator, denominator);
    numerator /= divisor, denominator /= divisor;

    return `${numerator}/${denominator}"`
}

function greatestCommonDenominator(a: number, b: number): number {
    if (b < 0.001) return a;

    return greatestCommonDenominator(b, Math.floor(a % b));
}