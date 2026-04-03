import {defineStore} from 'pinia'
import { ref, computed } from 'vue'
import {recipeSearchByCategories, recipeSearchByQuery,
     recipeSearchByIngredients} from '@/api/gastronomy_generated/sdk.gen'
import type { RecipeOperationResult } from '../types/recipeTypes';
import { ApiGastronomyError } from '@/api/types';

export const useRecipesStore = defineStore('recipes', () => 
{

    

    async function searchByCategories(categories: Array<string>, isAndLogic: boolean) : Promise<RecipeOperationResult>
    {
        try {
            var responce = await recipeSearchByCategories(
                {
                    body:{
                        include: categories,
                        includeLogicAnd: isAndLogic
                    }
                }
            );
            return {isSucces: true, recipes: responce.data?.recipes!};
        } catch (error) {
            if (error instanceof ApiGastronomyError)
                return {isSucces: false, error: error.message, recipes: null};
            else
                throw;
        }
    }   

});