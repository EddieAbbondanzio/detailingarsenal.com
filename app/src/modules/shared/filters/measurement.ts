import { Measurement } from "@/api";

export function measurement(val: Measurement | null): string {
    if (val == null) {
        return '';
    }

    return `${val.amount}${val.unit}`
}