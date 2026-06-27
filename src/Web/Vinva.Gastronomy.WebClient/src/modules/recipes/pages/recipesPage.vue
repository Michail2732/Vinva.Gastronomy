<template>
    <div>
        <Toast/>
        <Toolbar class="recipes-toolbar-container">             
            <template #start>
                <Button icon="pi pi-filter-fill"
                        v-tooltip.top="'Фильтрация'"/>
                <IconToggleButton onIcon="pi pi-bookmark-fill" 
                                  v-model="showCategories"
                                  v-tooltip.top="'Фильтрация по категориям'"
                                  class="categories-toggle-btn"
                                  offIcon="pi pi-bookmark"/>                
            </template>
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
        <div class="cards-container">
            <RecipeCard v-for="(item, index) in filtredRecipes" :key="item.id"
                        @click="goToRecipe(filtredRecipes[index]?.id!)"
                        v-model="filtredRecipes[index]"
                        class="recipe-card">                
            </RecipeCard>            
        </div>
    </div>    
</template>
<script setup lang="ts">
import {ref, computed} from 'vue'
import {useRouter} from 'vue-router'
import RecipeCard from '../components/recipeCard.vue'
import {useRecipesStore} from '../stores/recipesStore'
import type { RecipeCardViewModel } from '../types/recipeTypes';
import IconToggleButton from '@/modules/ui/components/iconToggleButton.vue'
import { useToast } from 'primevue';

const router = useRouter();
const toasts = useToast();
const showCategories = ref(true);
const recipeStore = useRecipesStore(); 
const recipeCardVms = ref<RecipeCardViewModel[]>([]);
const searchStr = ref('');
const filtredRecipes = computed(() => {
    return recipeCardVms.value.filter(a => 
    {
        let result = a.name?.toLowerCase().includes(searchStr.value.toLowerCase());        
        return result;
    })
});

function filterRecipes()
{
    recipeCardVms.value.filter(a => 
    {
        let result = a.name?.toLowerCase().includes(searchStr.value.toLowerCase());        
        return result;
    })
}

function goToRecipe(id: string) 
{
  router.push(`/recipe/${id}`);  
};

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
        recipeCardVms.value = recipesRes.data;        
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
                background-color: var(--p-surface-400);
                border-color: var(--p-surface-400);                
                .p-togglebutton-content
                {                                        
                    background: none;                    
                }
            }
        }        
    }

    .recipes-toolbar-container
    {
        margin: 2rem auto 1rem auto;        
        width: fit-content;     
        border: 0px;   
        .categories-toggle-btn
        {
            margin: 0 0 0 .4rem;
        }
        :deep(.p-inputtext)            
        {
            border-radius: 1rem;
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
            background-color: var(--p-primary-50);
            transition: transform 0.3s ease; 
            &:hover
            {
                background-color: var(--p-primary-100);
                transform: scale(1.1);
                cursor: grab;
            }
        }
    }
</style>