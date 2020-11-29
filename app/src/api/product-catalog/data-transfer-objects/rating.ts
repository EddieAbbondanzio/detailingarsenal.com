import { Stars } from './stars';

/**
 * Star rating, and the number of reviews the stars are based off.
 */
export class Rating {
    constructor(public stars: Stars, public reviewCount: number) { }
}