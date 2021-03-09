import { Stars } from './stars';

/**
 * Star rating, and the number of reviews the stars are based off.
 */
export interface Rating {
    stars: Stars;
    reviewCount: number;
}
