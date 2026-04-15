import type { CategoryDto, RecipeDto, RecipeIngredientDto, RecipeStepDto } from "@/api/gastronomy_generated";

export interface RecipeCardViewModel extends RecipeDto
{    
    
};

export interface RecipeStepViewModel extends RecipeStepDto
{
    
}


export interface RecipeDetailsViewModel extends RecipeDto
{    
    
};

export interface RecipeHeaderViewModel extends Pick<RecipeDto, 'id' | 'name' | 'description' | 'comment' | 'cookingTime' | 'categories'>
{

}

export interface RecipeIngredientViewModel extends RecipeIngredientDto
{

}

export interface CategoryViewModel extends CategoryDto
{

}

export interface CategoryRecipesViewModel
{
    recipes: Array<RecipeCardViewModel>;
    category: string;
};