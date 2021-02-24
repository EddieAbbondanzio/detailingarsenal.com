import { Paging } from './paging';

export interface PagedArray<T> {
    paging: {
        pageNumber: number;
        pageSize: number;
        total: number;
    };
    values: T[];
}
