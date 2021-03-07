import { PagingOptions } from './paging-options';

export interface Paging {
    pageNumber: number;
    pageSize: number;
    pageCount: number;
    total: number;
}

export interface PagedArray<T> {
    paging: Paging;
    values: T[];
}
