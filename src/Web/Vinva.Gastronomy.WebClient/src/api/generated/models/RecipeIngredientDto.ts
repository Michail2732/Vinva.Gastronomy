/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { IngredientQuantityDto } from './IngredientQuantityDto';
export type RecipeIngredientDto = {
    ingredientId: string;
    ingredientName: string | null;
    isRequired?: boolean;
    quantities: Array<IngredientQuantityDto> | null;
};

