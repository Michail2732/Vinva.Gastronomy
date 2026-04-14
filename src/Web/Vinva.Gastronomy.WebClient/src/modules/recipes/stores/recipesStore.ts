import {defineStore} from 'pinia'
import { ref, computed } from 'vue'
import {recipeSearchByCategories, 
        recipeSearchByQuery, 
        recipeSearchByIngredients,
        categorySearch } from '@/api/gastronomy_generated/sdk.gen'
import { ApiGastronomyError, type ApiDataResult } from '@/api/types';
import type { CategoryDto, RecipeDto } from '@/api/gastronomy_generated';
import type { CategoryRecipesViewModel, CategoryViewModel, RecipeCardViewModel, RecipeDetailsViewModel } from '../types/recipeTypes';

export const useRecipesStore = defineStore('recipes', () => 
{            
    async function getRecipeCategories() : Promise<ApiDataResult<Array<CategoryViewModel>>>
    {
        try {
            var responce = await categorySearch(
            {
                body: {
                    query: {
                        conditions: [
                            {
                                logic: 'Or',
                                operator: 'Equals',
                                field: 'Type',
                                value: 'Recipe'
                            }
                        ]
                    }                        
                }
            });
            return {isSuccess: true, data: responce.data?.items!};
        } catch (error) {
            if (error instanceof ApiGastronomyError)
                return {isSuccess: false, error: error.message};
            else
                throw error;
        }
    }

    async function getRecipesByCategories(categories: Array<string>) : Promise<ApiDataResult<Array<CategoryRecipesViewModel>>>
    {
        try {
            var responce = await recipeSearchByCategories(
            {
                body: {                    
                    include: categories,
                    includeLogicAnd: false
                }
            });
            const recipes = responce.data?.recipes!;
            if (!recipes)
                return {isSuccess: true, data: []};

            const categoryRecipes = new Array<CategoryRecipesViewModel>();            
            for (const category of categories) 
            {
                var matchRecipes = recipes.filter(a => a.categories?.find(b => b.id == category));
                categoryRecipes.push(
                    {
                        category: category,
                        recipes: matchRecipes
                    }
                )
            }            
            return {isSuccess: true, data: categoryRecipes};
        } catch (error) {
            if (error instanceof ApiGastronomyError)
                return {isSuccess: false, error: error.message};
            else
                throw error;
        }
    }

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
        getRecipeCategories, 
        getRecipes,
        getRecipeById,
        getRecipesByCategories
    }
})