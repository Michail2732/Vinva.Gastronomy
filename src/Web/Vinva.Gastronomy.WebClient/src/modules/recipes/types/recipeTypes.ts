import type { CategoryDto, RecipeDto } from "@/api/gastronomy_generated";

export interface RecipeCardViewModel extends RecipeDto
{    
    
};


export interface RecipeDetailsViewModel extends RecipeDto
{    
    
};

export interface CategoryViewModel extends CategoryDto
{

}

export interface CategoryRecipesViewModel
{
    recipes: Array<RecipeCardViewModel>;
    category: string;
};