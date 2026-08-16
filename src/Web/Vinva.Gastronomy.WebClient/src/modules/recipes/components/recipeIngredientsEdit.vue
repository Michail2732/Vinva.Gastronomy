<template>
    <div class="ingredients col-12 md:col-8 flex flex-column gap-4 mt-3">
        <span class="font-bold block">Список ингредиентов</span>
        <ListBox v-model="selectedIngredient"
                :options="recipeIngredients"                                
                scrollHeight="200px"
                listStyle="height:200px"
                optionLabel="ingredientName"
                class="m-0" />
        <div class="flex justify-left flex-wrap gap-2 mb-1">
            <Button icononly rounded  severity="success"
                    @click="addIngredient">
                <i class="pi block pi-plus w-1rem h-1rem"></i>
            </Button>
            <Button icononly rounded  severity="danger"
                    :disabled="!selectedIngredient"
                    @click="removeIngredient">
                <i class="pi pi-trash"></i>
            </Button>
            <Button icononly rounded  severity="secondary"
                    :disabled="!selectedIngredient"
                    @click="editIngredient">
                <span class="pi pi-pencil"></span>
            </Button>
        </div>
       <Dialog v-model:visible="ingredientEditorVisible" modal header="Ингредиент"
               class="xl:w-3 lg:w-5 md:w-6 w-11">
            <IngredientEditor :ingredient="selectedIngredient"
                              :mode="editMode"
                              @submit="ingredientChangeSubmit" 
                              class="w-12"/>
       </Dialog>
    </div>
</template>
<script setup lang="ts">
import { ref } from 'vue';
import type {RecipeIngredientViewModel} from '../types/recipeTypes.ts'
import  IngredientEditor  from './recipeIngredientEdit.vue'
import { DtoState } from '@/api/gastronomy_generated/types.gen.ts';

const ingredientEditorVisible = ref(false);
const selectedIngredient = ref<RecipeIngredientViewModel | null>(null);
const editMode = ref<'create' | 'edit'>('create');
const props = defineProps<{recipeIngredients: RecipeIngredientViewModel[]}>();
const recipeIngredients = ref(props.recipeIngredients || []);



const addIngredient = () => {
    editMode.value = 'create';
    selectedIngredient.value = null;    
    ingredientEditorVisible.value = true;
}

const editIngredient = () => {
    editMode.value = 'edit';
    ingredientEditorVisible.value = true;
}

const removeIngredient = () => {
    const removeIndex = recipeIngredients.value.indexOf(selectedIngredient.value!);
    if (removeIndex)
    {
        recipeIngredients.value.splice(removeIndex, 1);
    }    
}

const ingredientChangeSubmit = (ingredient: RecipeIngredientViewModel) =>
{
    if (!recipeIngredients.value.includes(ingredient))
        recipeIngredients.value.push(ingredient);
    ingredientEditorVisible.value = !ingredientEditorVisible.value;
}
</script>
<style scoped lang="scss">    
    .ingredients
    {                
        margin: 0 auto;        
    }        
</style>