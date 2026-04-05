<template>
    <div>
        <div class="cards-container">
            <RecipeCard v-for="(item, index) in recipeCardVms" :key="item.id"
                  v-model="recipeCardVms[index]"
                  class="recipe-card">                
            </RecipeCard>            
        </div>
    </div>    
</template>
<script setup lang="ts">
import {ref} from 'vue'
import RecipeCard from '../components/recipeCard.vue'
import {useRecipesStore} from '../stores/recipesStore'
import type { CategoryRecipesViewModel, RecipeCardViewModel } from '../types/recipeTypes';

const recipeStore = useRecipesStore(); 
const recipeCardVms = ref<RecipeCardViewModel[]>();

async function loadData()
{
    try 
    {
        const recipes = await recipeStore.getRecipes();
        if (!recipes.isSuccess)
        {
            alert("Не удалось получить список рецептов");
            return;
        }          
        recipeCardVms.value = recipes.data;
    } catch (error) {
        alert(error);
    }    
}

await loadData();


</script>
<style scoped lang="scss">
    .cards-container
    {
        margin: 1rem auto;
        display: flex;
        flex-direction: row;
        justify-content: center;
        flex-wrap: wrap;
        .recipe-card
        {
            margin: 1rem;
            width: 300px;
        }
    }
</style>