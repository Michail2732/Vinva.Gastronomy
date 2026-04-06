<template>
    <div>
        <Toolbar class="recipes-toolbar-container">             
            <template #center>
                <div>
                    <IconField>
                        <InputIcon>
                            <i class="pi pi-search" />
                        </InputIcon>
                        <InputText v-model="searchStr"
                                   @input="filterRecipes"
                                   autofocus
                                   placeholder="Поиск рецепта"/>
                    </IconField>                    
                </div>                
            </template>             
        </Toolbar>        
        <div class="categories-container">                        
            <SelectButton v-model="value" :options="options" optionLabel="name" multiple aria-labelledby="multiple" />
        </div>                    
        <div class="cards-container">
            <RecipeCard v-for="(item, index) in filtredRecipes" :key="item.id"
                  v-model="filtredRecipes[index]"
                  class="recipe-card">                
            </RecipeCard>            
        </div>
    </div>    
</template>
<script setup lang="ts">
import {ref, computed} from 'vue'
import RecipeCard from '../components/recipeCard.vue'
import {useRecipesStore} from '../stores/recipesStore'
import type { CategoryRecipesViewModel, RecipeCardViewModel } from '../types/recipeTypes';

const value = ref(null);
const options = ref([
    { name: 'Option 1', value: 1 },
    { name: 'Option 2', value: 2 },
    { name: 'Option 3', value: 3 },
    { name: 'Option 3', value: 3 },
    { name: 'Option 3', value: 3 },
    { name: 'Option 3', value: 3 },
    { name: 'Option 3', value: 3 },
]);

const recipeStore = useRecipesStore(); 
const recipeCardVms = ref<RecipeCardViewModel[]>([]);
const searchStr = ref('');
const filtredRecipes = computed(() =>{
    return recipeCardVms.value.filter(a => 
        a.name?.toLowerCase().includes(searchStr.value.toLowerCase()))
});

function filterRecipes()
{
    recipeCardVms.value.filter(a => a.name?.includes(searchStr.value))
}

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

loadData();


</script>
<style scoped lang="scss">
    .recipes-toolbar-container
    {
        margin: 2rem auto 1rem auto;        
        width: fit-content;
        .categories-container
        {
            display: block;
        }
    }

    .cards-container
    {        
        margin: 1rem auto;
        display: flex;
        flex-direction: row;
        justify-content: center;
        flex-wrap: wrap;
        max-width: 1800px;
        .recipe-card
        {
            margin: 1rem;
            width: 300px;
            background-color: #f9f9f9;
        }
    }
</style>