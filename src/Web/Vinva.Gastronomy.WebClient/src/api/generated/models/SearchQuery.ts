/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { Condition } from './Condition';
import type { Sorting } from './Sorting';
export type SearchQuery = {
    take?: number;
    skip?: number;
    sort?: Sorting;
    conditions?: Array<Condition> | null;
};

