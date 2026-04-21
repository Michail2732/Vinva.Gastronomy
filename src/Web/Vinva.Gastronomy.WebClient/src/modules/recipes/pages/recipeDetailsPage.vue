<template>
   <div class="recipe-details-page-root">
        <div v-if="recipe" class="recipe-details-container">
            <Toast />
            <RecipeHeader :recipeHeader="recipe"/>
            <div class="grid">
                <div class="col-12 lg:col-7 ">                    
                    <RecipeSteps :steps="recipe?.steps"
                                 class="recipes-steps"/>
                </div>
                <div class="col-12 lg:col-5">
                    <RecipeIngredients :ingredients="recipe?.ingredients"
                                        class="ingredients"
                                       :ingredientsComment="recipe?.ingredientComment"/>
                </div>
            </div>
            <div class="recipe-comments-container grid">
                <div class="col-12 md:col-6 lg:col-4">                    
                        <Comment :title="'Коментарий к использованию'"
                                 class="recipe-comment"
                                 :content="recipe?.usageComment"/>
                </div>                
                <div class="col-12 md:col-6 lg:col-4">                    
                        <Comment :title="'Коментарий к хранению'"
                                 class="recipe-comment"
                                 :content="recipe?.storageComment"/>                    
                </div>                             
            </div>
            <div>

            </div>
        </div>
    </div>
</template>
<script setup lang="ts">
import { useRecipesStore } from '../stores/recipesStore';
import Comment from '@/modules/ui/components/comment.vue'
import { useToast } from 'primevue';
import { watch, ref } from 'vue';
import { useRoute } from 'vue-router';
import RecipeSteps from '../components/recipeSteps.vue'
import RecipeHeader from '../components/recipeHeader.vue'
import type { RecipeDetailsViewModel } from '../types/recipeTypes';
import RecipeIngredients from '../components/recipeIngredients.vue';

const recipe = ref<RecipeDetailsViewModel | null>(null);
const recipeStore = useRecipesStore();
const route = useRoute();
const toasts = useToast();

async function loadRecipe(id: string)
{
    try 
    {
        const result = await recipeStore.getRecipeById(id);
        if (!result.isSuccess)
            toasts.add({severity: 'error', summary: 'Ошибка', detail: result.error, life: 3500});
        else 
            recipe.value = result.data;
    } 
    catch (error)
    {
        toasts.add({severity: 'error', summary: 'Ошибка', detail: error, life: 3500});            
    }        
}

loadRecipe(route.params.id as string);

watch(
    () => route.params.id,
    (newId) => 
    {
        if (newId)    
        {
            loadRecipe(newId as string);
        }
    },
    {immediate: true}
);

</script>
<style scoped lang="scss">
@use "../../../assets/variables.scss" as vars;

.recipe-details-page-root
{
    background-color: vars.$violet-100-color;
    min-height: 100vh;
    .recipe-comments-container
    {        
        .recipe-comment
        {                       
            margin-top: .5rem;              
            margin-left: .5rem;
            margin-right: .5rem;
            background-color: vars.$violet-200-color;
            border-left: 0;               
            font-size: 2.3rem;   
            color: vars.$dark-600-color;
            :deep(.comment-header-icon)
            {
                display: block;
                margin-bottom: 1rem;
                color: vars.$sea-green-800-color;
                font-weight: 700;
            }
            :deep(.comment-text)
            {
                color: vars.$dark-400-color;
                font-size: .36em;
                margin-top: 1.5rem;
            }
        }
    }    

    .recipe-details-container
    {     
        max-width: 1200px;
        margin: 0 auto;        
        .recipes-steps
        {            
            margin: 1rem;
        }
        .ingredients
        {            
            margin: .5rem;
        }
    }
}    
</style>