import { PadCategory } from '@/api';

export class Filter {
    constructor(public brands: string[] = [], public series: string[] = [], public category: PadCategory[] = []) {}

    get isEmpty() {
        return this.brands.length == 0 && this.series.length == 0 && this.category.length == 0;
    }
}
