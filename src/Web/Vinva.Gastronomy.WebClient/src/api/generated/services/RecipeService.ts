/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { AddIngredientCommand } from '../models/AddIngredientCommand';
import type { AddRecipeCategoriesCommand } from '../models/AddRecipeCategoriesCommand';
import type { AddStepsCommand } from '../models/AddStepsCommand';
import type { CreateRecipeCommand } from '../models/CreateRecipeCommand';
import type { CreateRecipeResponce } from '../models/CreateRecipeResponce';
import type { GetRecipeByCategoryRequest } from '../models/GetRecipeByCategoryRequest';
import type { GetRecipeByCategoryResponce } from '../models/GetRecipeByCategoryResponce';
import type { GetRecipeByIngredientsRequest } from '../models/GetRecipeByIngredientsRequest';
import type { GetRecipeByIngredientsResponce } from '../models/GetRecipeByIngredientsResponce';
import type { GetRecipesByFilterQuery } from '../models/GetRecipesByFilterQuery';
import type { GetRecipesByFilterQueryResponse } from '../models/GetRecipesByFilterQueryResponse';
import type { RemoveIngredientsCommand } from '../models/RemoveIngredientsCommand';
import type { RemoveRecipeCategoryCommand } from '../models/RemoveRecipeCategoryCommand';
import type { RemoveStepsCommand } from '../models/RemoveStepsCommand';
import type { ReorderStepsCommand } from '../models/ReorderStepsCommand';
import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';
export class RecipeService {
    /**
     * @param requestBody
     * @returns any OK
     * @throws ApiError
     */
    public static patchApiRecipesAddIngredients(
        requestBody?: AddIngredientCommand,
    ): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'PATCH',
            url: '/api/Recipes/AddIngredients',
            body: requestBody,
            mediaType: 'application/json',
        });
    }
    /**
     * @param requestBody
     * @returns any OK
     * @throws ApiError
     */
    public static patchApiRecipesAddCategories(
        requestBody?: AddRecipeCategoriesCommand,
    ): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'PATCH',
            url: '/api/Recipes/AddCategories',
            body: requestBody,
            mediaType: 'application/json',
        });
    }
    /**
     * @param requestBody
     * @returns any OK
     * @throws ApiError
     */
    public static patchApiRecipesAddSteps(
        requestBody?: AddStepsCommand,
    ): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'PATCH',
            url: '/api/Recipes/AddSteps',
            body: requestBody,
            mediaType: 'application/json',
        });
    }
    /**
     * @param requestBody
     * @returns CreateRecipeResponce OK
     * @throws ApiError
     */
    public static postApiRecipesCreate(
        requestBody?: CreateRecipeCommand,
    ): CancelablePromise<CreateRecipeResponce> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/Recipes/Create',
            body: requestBody,
            mediaType: 'application/json',
        });
    }
    /**
     * @param requestBody
     * @returns GetRecipeByCategoryResponce OK
     * @throws ApiError
     */
    public static postApiRecipesSearchByCategories(
        requestBody?: GetRecipeByCategoryRequest,
    ): CancelablePromise<GetRecipeByCategoryResponce> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/Recipes/SearchByCategories',
            body: requestBody,
            mediaType: 'application/json',
        });
    }
    /**
     * @param requestBody
     * @returns GetRecipeByIngredientsResponce OK
     * @throws ApiError
     */
    public static postApiRecipesSearchByIngredients(
        requestBody?: GetRecipeByIngredientsRequest,
    ): CancelablePromise<GetRecipeByIngredientsResponce> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/Recipes/SearchByIngredients',
            body: requestBody,
            mediaType: 'application/json',
        });
    }
    /**
     * @param requestBody
     * @returns GetRecipesByFilterQueryResponse OK
     * @throws ApiError
     */
    public static postApiRecipesSearchByQuery(
        requestBody?: GetRecipesByFilterQuery,
    ): CancelablePromise<GetRecipesByFilterQueryResponse> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/Recipes/SearchByQuery',
            body: requestBody,
            mediaType: 'application/json',
        });
    }
    /**
     * @param recipeId
     * @returns any OK
     * @throws ApiError
     */
    public static deleteApiRecipesRemove(
        recipeId?: string,
    ): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'DELETE',
            url: '/api/Recipes/Remove',
            query: {
                'recipeId': recipeId,
            },
        });
    }
    /**
     * @param requestBody
     * @returns any OK
     * @throws ApiError
     */
    public static patchApiRecipesRemoveIngredients(
        requestBody?: RemoveIngredientsCommand,
    ): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'PATCH',
            url: '/api/Recipes/RemoveIngredients',
            body: requestBody,
            mediaType: 'application/json',
        });
    }
    /**
     * @param requestBody
     * @returns any OK
     * @throws ApiError
     */
    public static patchApiRecipesRemoveCategories(
        requestBody?: RemoveRecipeCategoryCommand,
    ): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'PATCH',
            url: '/api/Recipes/RemoveCategories',
            body: requestBody,
            mediaType: 'application/json',
        });
    }
    /**
     * @param requestBody
     * @returns any OK
     * @throws ApiError
     */
    public static patchApiRecipesRemoveSteps(
        requestBody?: RemoveStepsCommand,
    ): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'PATCH',
            url: '/api/Recipes/RemoveSteps',
            body: requestBody,
            mediaType: 'application/json',
        });
    }
    /**
     * @param requestBody
     * @returns any OK
     * @throws ApiError
     */
    public static patchApiRecipesReorderSteps(
        requestBody?: ReorderStepsCommand,
    ): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'PATCH',
            url: '/api/Recipes/ReorderSteps',
            body: requestBody,
            mediaType: 'application/json',
        });
    }
}
