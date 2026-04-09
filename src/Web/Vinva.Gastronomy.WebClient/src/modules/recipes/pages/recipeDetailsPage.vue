<template>
    <div class="recipe-details-container">
        <div>
            <div>
                <Image/>
            </div>
            <div>

            </div>
        </div>
        <div>
            <div>

            </div>
            <div>

            </div>
        </div>
        <div>

        </div>
        <div>

        </div>
    </div>
</template>
<script setup lang="ts">
import { useRecipesStore } from '../stores/recipesStore';
import { useToast } from 'primevue';
import { watch } from 'vue';
import { useRoute } from 'vue-router';

const recipeStore = useRecipesStore();
const route = useRoute();
const toasts = useToast();

async function loadRecipe(id: string)
{
    try 
    {
        const recipe = await recipeStore.getRecipeById(id);    
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
<style lang="scss">
    $extra-light-violet-background-color: #f8f9ff;
    $light-violet-background-color: #eef4ff;
    $light-green-color: #e7f8f2;
    $dark-blue-color: #121c28;
    $dark-green-color: #006c49;
    $middle-green-color: #5e8f76;
    $light-swamp-color: #66726e;

    .recipe-details-container
    {
        background-color: $extra-light-violet-background-color;
    }
</style>