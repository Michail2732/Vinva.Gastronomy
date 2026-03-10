/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { IngredientCategoryDto } from './IngredientCategoryDto';
export type IngredientDto = {
    id: string;
    name: string | null;
    description?: string | null;
    usageComment?: string | null;
    photoId?: string | null;
    recipeId?: string | null;
    categories?: Array<IngredientCategoryDto> | null;
};

