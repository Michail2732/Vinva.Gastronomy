import type { RecipeDto } from "@/api/gastronomy_generated";
import type { ApiOperationResult } from "@/api/types";

export interface RecipeInfo
{
    id: string;
    name: string;
    imageSrc: string | undefined;
    description: string | null;
}

export interface RecipeOperationResult extends ApiOperationResult
{
    recipes: Array<RecipeDto> | null;    
}