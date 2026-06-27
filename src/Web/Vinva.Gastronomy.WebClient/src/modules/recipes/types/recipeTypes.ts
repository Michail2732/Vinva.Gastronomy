import type { RecipeDto, RecipeIngredientDto, RecipeStepDto } from "@/api/gastronomy_generated";

export interface RecipeCardViewModel extends RecipeDto
{    
    
};

export interface RecipeStepViewModel extends RecipeStepDto
{
    
}


export interface RecipeDetailsViewModel extends RecipeDto
{    
    
};

export interface RecipeHeaderViewModel extends Pick<RecipeDto, 'id' | 'name' | 'description' | 'comment' | 'cookingTime' | 'properties'>
{

}

export interface RecipeIngredientViewModel extends RecipeIngredientDto
{

}

export interface CategoryRecipesViewModel
{
    recipes: Array<RecipeCardViewModel>;
    category: string;
};