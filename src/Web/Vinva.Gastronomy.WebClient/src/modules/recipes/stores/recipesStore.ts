import {defineStore} from 'pinia'
import { ref, computed } from 'vue'
import {recipeSearchByQuery, recipeSearchByIngredients} from '@/api/gastronomy_generated/sdk.gen'
import { ApiGastronomyError, type ApiDataResult } from '@/api/types';
import type { RecipeDto } from '@/api/gastronomy_generated';
import type { CategoryRecipesViewModel, RecipeCardViewModel, RecipeDetailsViewModel } from '../types/recipeTypes';

export const useRecipesStore = defineStore('recipes', () => 
{                
    async function getRecipeById(id: string) : Promise<ApiDataResult<RecipeDetailsViewModel>>
    {
         try {
            var responce = await recipeSearchByQuery(
            {
                body: {                    
                    query: 
                    {
                        conditions: 
                        [
                            {
                                field: 'Id',
                                logic: 'Or',
                                operator: 'Equals',
                                value: id
                            }
                        ]
                     }                    
                }
            });
            const recipe = responce.data?.recipes?.at(0);
            if (!recipe)
                return {isSuccess: false, error: `Не удалось найти рецепт (id = ${id})`};
            
            return {isSuccess: true, data: recipe};
        } catch (error) {
            if (error instanceof ApiGastronomyError)
                return {isSuccess: false, error: error.message};
            else
                throw error;
        }
    }


    async function getRecipes() : Promise<ApiDataResult<Array<RecipeCardViewModel>>>
    {
        try {
            var responce = await recipeSearchByQuery(
            {
                body: {                    
                    query: { }                    
                }
            });
            const recipes = responce.data?.recipes!;
            if (!recipes)
                return {isSuccess: true, data: []};
            
            return {isSuccess: true, data: recipes};
        } catch (error) {
            if (error instanceof ApiGastronomyError)
                return {isSuccess: false, error: error.message};
            else
                throw error;
        }
    }

    return {        
        getRecipes,
        getRecipeById        
    }
})