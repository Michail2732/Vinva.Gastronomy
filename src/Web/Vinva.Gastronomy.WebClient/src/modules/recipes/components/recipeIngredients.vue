<template>
    <div class="ingredients-container">
        <label class="ingredient-header-text">Ингредиенты</label>
        <ul class="ingredients-list-container">
            <li v-for="item in ingredients" class="ingredient-container">
                <span class="ingredient-name-text">{{ item.ingredientName }}</span>
                <span class="ingredient-value-text">{{ getAsStringQuantities(item.quantities) }}</span>
            </li>
        </ul>
        <Comment :title="commentTitle"
                 :content="ingredientsComment"/>        
    </div>
</template>

<script setup lang="ts">
import type { RecipeIngredientViewModel } from '../types/recipeTypes';
import type { IngredientQuantityDto } from '@/api/gastronomy_generated';
import Comment from '@/modules/ui/components/comment.vue'
import {ref} from 'vue'

function getAsStringQuantities(quantiies: IngredientQuantityDto[] | null) : string
{
    if (!quantiies)
        return '';
    return quantiies.map(a => `${a.quantity} ${a.measure}`).join('/');
}

const commentTitle = ref('СОВЕТЫ ПО ИНГРЕДИЕНТАМ');
const props = defineProps<{
    ingredients: RecipeIngredientViewModel[] | null | undefined,
    ingredientsComment?: string | null | undefined
}>();
const ingredients = props.ingredients;

</script>
<style scoped lang="scss">
@use "../../../assets/variables.scss" as *;

.ingredients-container
{
    font-family: 'Inter';
    background-color: white;
    border-radius: 1.5rem;
    padding: 2rem;    

    .ingredient-header-text
    {
        line-height: 2rem;
        letter-spacing: -.01rem;
        font-size: 1.5rem;
        font-weight: 700;
    }
    .ingredients-list-container
    {
        list-style-type: none;
        padding: 0;
        margin-top: 2rem;
        .ingredient-container
        {        
            margin: 1rem 0;
            display: flex;
            justify-content: space-between;
            align-items: center;
            .ingredient-name-text, .ingredient-value-text        
            {
                line-height: 1.5rem;
                letter-spacing: 0rem;
                font-size: 1rem;            
            }
            .ingredient-name-text
            {
                font-weight: 200;
            }
            .ingredient-value-text
            {
                font-weight: 600;
                white-space: nowrap;                
            }
        }
    }    
}
</style>