<template>
    <div class="recipe-editor-container">
        <RecipeHeaderEdit />
        <RecipeIngredientsEdit :recipe-ingredients="ingredients"/>
        <RecipeStepsEdit/>
    </div>
</template>
<script setup lang="ts">
import { ref } from 'vue';
import RecipeHeaderEdit from '../components/recipeHeaderEdit.vue';
import RecipeIngredientsEdit from '../components/recipeIngredientsEdit.vue';
import type {RecipeIngredientViewModel} from '../types/recipeTypes.ts'
import RecipeStepsEdit from '../components/recipeStepsEdit.vue';

const ingredients = ref<RecipeIngredientViewModel[]>([]);

</script>
<style lang="scss">
    .recipe-editor-container
    {
        max-width: 1200px;
        margin: 0 auto;
    }
</style>