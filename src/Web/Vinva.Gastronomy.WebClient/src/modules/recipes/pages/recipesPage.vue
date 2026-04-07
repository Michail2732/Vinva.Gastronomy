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
            <SelectButton v-model="selectedCategories" 
                          :options="categories"                          
                          optionLabel="name"                          
                          multiple
                          aria-labelledby="multiple" />
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
import type { CategoryViewModel, RecipeCardViewModel } from '../types/recipeTypes';
import { useToast } from 'primevue';


const toasts = useToast();
const categories = ref<{name: string, value: CategoryViewModel}[]>();
const selectedCategories = ref<{name: string, value: CategoryViewModel}[]>([]);
const recipeStore = useRecipesStore(); 
const recipeCardVms = ref<RecipeCardViewModel[]>([]);
const searchStr = ref('');
const filtredRecipes = computed(() => {
    return recipeCardVms.value.filter(a => 
    {
        let result = a.name?.toLowerCase().includes(searchStr.value.toLowerCase());
        if (selectedCategories.value)
        {
            let isMatchRecipe = true;
            for (const category of selectedCategories.value) 
            {
                isMatchRecipe &&= a.categories?.some(b => b.id == category.value.id!) == true;
            }
            result &&= isMatchRecipe;
        }
        return result;
    })
});

function filterRecipes()
{
    recipeCardVms.value.filter(a => 
    {
        let result = a.name?.toLowerCase().includes(searchStr.value.toLowerCase());
        if (selectedCategories.value)
        {
            let isMatchRecipe = true;
            for (const category of selectedCategories.value) 
            {
                isMatchRecipe &&= a.categories?.some(b => b.id == category.value.id!) == true;
            }
            result &&= isMatchRecipe;
        }
        return result;
    })
}

async function loadData()
{
    try 
    {
        const recipesRes = await recipeStore.getRecipes();        
        if (!recipesRes.isSuccess)
        {
            toasts.add({severity: 'error', summary: 'Ошибка', 
                detail: "Не удалось получить список рецептов", life: 3500});            
            return;
        }     
        const categoriesRes = await recipeStore.getRecipeCategories();
        if (!categoriesRes.isSuccess)     
        {
            toasts.add({severity: 'error', summary: 'Ошибка', 
                detail: "Не удалось получить список категорий", life: 3500});            
            return;
        }
        recipeCardVms.value = recipesRes.data;
        categories.value = categoriesRes.data.filter(a => a.name).map(a => 
        {
            return {name: a.name!, value: a};
        });
    } catch (error) {
        toasts.add({severity: 'error', summary: 'Ошибка', detail: error, life: 3500});            
    }    
}

loadData();


</script>
<style scoped lang="scss">
    .categories-container
    {
        display: block;
        margin: 0 auto;
        width: 70%;
        max-width: 1000px;
        :deep(.p-selectbutton)
        {
            display: flex;
            flex-wrap: wrap;
            justify-content: center;            
            .p-togglebutton
            {
                border-radius: 1rem !important;
                margin: .4rem;
            }
            .p-togglebutton-checked
            {
                background-color: var(--p-secondary-400);
                border-color: var(--p-secondary-400);
                color: var(--p-secondary-contrast);
                .p-togglebutton-content
                {                                        
                    background: none;                 
                    .p-togglebutton-label   
                    {
                        color: var(--p-secondary-100);
                    }
                }
            }
        }        
    }

    .recipes-toolbar-container
    {
        margin: 2rem auto 1rem auto;        
        width: fit-content;        
        .search-recipe-input
        {
            border: 0px;
            .p-inputtext 
            {
                border-radius: 1rem;
            }
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