/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { CreateIngredientCommand } from '../models/CreateIngredientCommand';
import type { CreateIngredientResponse } from '../models/CreateIngredientResponse';
import type { GetIngredientByIdResponce } from '../models/GetIngredientByIdResponce';
import type { RemoveIngredientCommand } from '../models/RemoveIngredientCommand';
import type { UpdateIngredientCommand } from '../models/UpdateIngredientCommand';
import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';
export class IngredientService {
    /**
     * @param requestBody
     * @returns CreateIngredientResponse OK
     * @throws ApiError
     */
    public static postApiIngredientsCreate(
        requestBody?: CreateIngredientCommand,
    ): CancelablePromise<CreateIngredientResponse> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/Ingredients/Create',
            body: requestBody,
            mediaType: 'application/json',
        });
    }
    /**
     * @param ingredientId
     * @returns GetIngredientByIdResponce OK
     * @throws ApiError
     */
    public static getApiIngredientsGetById(
        ingredientId?: string,
    ): CancelablePromise<GetIngredientByIdResponce> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/Ingredients/GetById',
            query: {
                'ingredientId': ingredientId,
            },
        });
    }
    /**
     * @param requestBody
     * @returns any OK
     * @throws ApiError
     */
    public static deleteApiIngredientsRemove(
        requestBody?: RemoveIngredientCommand,
    ): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'DELETE',
            url: '/api/Ingredients/Remove',
            body: requestBody,
            mediaType: 'application/json',
        });
    }
    /**
     * @param requestBody
     * @returns any OK
     * @throws ApiError
     */
    public static patchApiIngredientsUpdate(
        requestBody?: UpdateIngredientCommand,
    ): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'PATCH',
            url: '/api/Ingredients/Update',
            body: requestBody,
            mediaType: 'application/json',
        });
    }
}
