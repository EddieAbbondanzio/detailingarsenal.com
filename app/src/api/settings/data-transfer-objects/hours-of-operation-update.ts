export type HoursOfOperationUpdate = {
    id: string;
    days: { day: number; open: number; close: number; enabled: boolean }[];
};
