import {defineStore} from 'pinia'
import { recipeCreate, recipeDelete, recipeSearch, recipeUpdate } from '@/api/gastronomy_generated/sdk.gen'
import { ApiGastronomyError, type ApiDataResult } from '@/api/types';
import type {
    RecipeCardViewModel,
    RecipeCreatedViewModel,
    RecipeDetailsViewModel,
    RecipeDraftViewModel,
    RecipeSearchViewModel,
} from '../types/recipeTypes';
import {
    mapCreatedRecipeToViewModel,
    mapRecipeDraftToCreateCommand,
    mapRecipeDraftToUpdateCommand,
    mapRecipeDto,
    mapRecipeSearchToQuery,
} from '../types/recipeTypes.map';

export const useRecipesStore = defineStore('recipes', () => 
{                
    async function getRecipeById(id: string) : Promise<ApiDataResult<RecipeDetailsViewModel>>
    {
         try {
            const recipes = await getRecipes()
            if (!recipes.isSuccess)
                return recipes

            const recipe = recipes.data.find((item) => item.id === id)
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
        const result = await searchRecipesByCharacteristics({ filters: [] })
        if (!result.isSuccess)
            return result

        return { isSuccess: true, data: result.data }
    }

    async function createRecipe(draft: RecipeDraftViewModel): Promise<ApiDataResult<RecipeCreatedViewModel>> {
        try {
            const response = await recipeCreate({ body: mapRecipeDraftToCreateCommand(draft) })
            if (!response.data?.recipeId)
                return { isSuccess: false, error: 'Сервер не вернул созданный рецепт' }

            return { isSuccess: true, data: mapCreatedRecipeToViewModel(response.data.recipeId, response.data.name) }
        } catch (error) {
            if (error instanceof ApiGastronomyError)
                return {isSuccess: false, error: error.message};
            else
                throw error;
        }
    }

    async function updateRecipe(id: string, draft: RecipeDraftViewModel): Promise<ApiDataResult<void>> {
        try {
            await recipeUpdate({
                path: { id },
                body: mapRecipeDraftToUpdateCommand(id, draft),
            })
            return { isSuccess: true, data: undefined }
        } catch (error) {
            if (error instanceof ApiGastronomyError)
                return { isSuccess: false, error: error.message }
            throw error
        }
    }

    async function deleteRecipe(id: string): Promise<ApiDataResult<void>> {
        try {
            await recipeDelete({ path: { id } })
            return { isSuccess: true, data: undefined }
        } catch (error) {
            if (error instanceof ApiGastronomyError)
                return { isSuccess: false, error: error.message }
            throw error
        }
    }

    async function searchRecipesByCharacteristics(
        query: RecipeSearchViewModel,
    ): Promise<ApiDataResult<RecipeCardViewModel[]>> {
        try {
            const response = await recipeSearch({ body: mapRecipeSearchToQuery(query) })
            return {
                isSuccess: true,
                data: (response.data?.recipes ?? []).map(mapRecipeDto),
            }
        } catch (error) {
            if (error instanceof ApiGastronomyError)
                return { isSuccess: false, error: error.message }
            throw error
        }
    }

    return {        
        getRecipes,
        getRecipeById,
        createRecipe,
        updateRecipe,
        deleteRecipe,
        searchRecipesByCharacteristics,
    }
})