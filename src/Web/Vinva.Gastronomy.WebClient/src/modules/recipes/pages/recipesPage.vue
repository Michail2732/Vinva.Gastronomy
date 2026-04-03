<template>
    <div>
        <div class="cards-container">
            <Card v-for="(item, index) in recipes" :key="item.id"
                  class="recipe-card">
                <template #content>
                    <Image :src="item.imageSrc"></Image>
                </template>
                <template #title>
                    {{ item.name }}
                </template>
            </Card>            
        </div>
    </div>    
</template>
<script setup lang="ts">
import type { RecipeInfo } from '../types/recipeTypes';
import {ref} from 'vue'

function generateTestRecipeInfo(): Array<RecipeInfo>
{
    const resultArray = new Array<RecipeInfo>(100);
    for (let i = 0; i < 100; i++) 
    {
        resultArray[i] = {
            id: String(i),
            name: `Тестовый рецепт ${i}`,
            imageSrc: undefined,
            description: `Тестовое описание ${i}`
        }            
    }
    return resultArray;
}

const recipes = ref(generateTestRecipeInfo());


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