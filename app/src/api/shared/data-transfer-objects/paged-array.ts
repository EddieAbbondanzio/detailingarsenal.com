import { Paging } from './paging';

export interface PagedArray<T> {
    paging: {
        pageNumber: number;
        pageSize: number;
        pageCount: number;
        total: number;
    };
    values: T[];
}
