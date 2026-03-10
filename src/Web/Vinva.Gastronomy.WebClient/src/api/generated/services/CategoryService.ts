/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { CreateCategoryCommand } from '../models/CreateCategoryCommand';
import type { CreateCategoryResponse } from '../models/CreateCategoryResponse';
import type { RemoveCategoryCommand } from '../models/RemoveCategoryCommand';
import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';
export class CategoryService {
    /**
     * @param requestBody
     * @returns CreateCategoryResponse OK
     * @throws ApiError
     */
    public static postApiCategoriesCreate(
        requestBody?: CreateCategoryCommand,
    ): CancelablePromise<CreateCategoryResponse> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/Categories/Create',
            body: requestBody,
            mediaType: 'application/json',
        });
    }
    /**
     * @param requestBody
     * @returns any OK
     * @throws ApiError
     */
    public static deleteApiCategoriesRemove(
        requestBody?: RemoveCategoryCommand,
    ): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'DELETE',
            url: '/api/Categories/Remove',
            body: requestBody,
            mediaType: 'application/json',
        });
    }
}
