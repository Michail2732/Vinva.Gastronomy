/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { Logic } from './Logic';
import type { Operator } from './Operator';
export type Condition = {
    logic: Logic;
    field: string | null;
    operator: Operator;
    value: any;
};

