/**
 * Format a time duration in minutes to Xh Ym format.
 * @param totalMinutes The minutes to format.
 */
export function duration(totalMinutes: number) {
    const minutes = totalMinutes % 60;
    const hours = (totalMinutes - minutes) / 60;

    if (hours > 0) {
        if (minutes > 0) {
            return `${hours}h ${minutes}m`;
        } else {
            return `${hours}h`;
        }
    } else {
        return `${minutes}m`;
    }
}
