/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { RecipeCategoryDto } from './RecipeCategoryDto';
import type { RecipeIngredientDto } from './RecipeIngredientDto';
import type { RecipeStepDto } from './RecipeStepDto';
export type RecipeDto = {
    id: string;
    name: string | null;
    description?: string | null;
    baseRecipe?: string | null;
    photoId?: string | null;
    videoId?: string | null;
    cookingTime?: string | null;
    cookingComment?: string | null;
    ingredientComment?: string | null;
    storageComment?: string | null;
    usageComment?: string | null;
    ingredients?: Array<RecipeIngredientDto> | null;
    categories?: Array<RecipeCategoryDto> | null;
    steps?: Array<RecipeStepDto> | null;
};

