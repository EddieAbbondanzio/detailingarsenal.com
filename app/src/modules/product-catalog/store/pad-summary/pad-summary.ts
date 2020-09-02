import { PadCategory, Stars } from '@/api';

/**
 * Summary used to display some pad info to the user on the pads page.
 */
export interface PadSummary {
    id: string;
    image?: { name: string; data: string };
    label: string; // SIZE BRAND SERIES NAME
    category: PadCategory;
    cut?: number;
    finish?: number;
    stars?: Stars;
    reviewCount: number;
    material: string;
    diameter: number;
    thickness: number;
    recommendedFor: string[];
};