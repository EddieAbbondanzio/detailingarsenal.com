import { PadCategory } from '@/api';

/**
 * Summary used to display some pad info to the user on the pads page.
 */
export interface PadSummary {
    id: string;
    image?: { name: string; data: string };
    brandName: string;
    name: string;
    seriesName: string;
    category: PadCategory;
    cut?: number;
    finish?: number;
    diameter: string;
    material: string;
    recommendedFor: string[];
};