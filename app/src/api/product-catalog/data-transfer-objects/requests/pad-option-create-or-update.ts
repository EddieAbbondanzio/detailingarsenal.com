export interface PadOptionCreateOrUpdate {
    padSizeId: string | null;
    padSizeIndex: number | null; // When we create options, pad sizes don't have ids yet.
    partNumber: string | null;
}