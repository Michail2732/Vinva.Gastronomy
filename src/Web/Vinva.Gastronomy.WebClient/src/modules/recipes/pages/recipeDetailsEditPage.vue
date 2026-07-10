<template>
    <div class="recipe-editor-container">
        <RecipeHeaderEdit />
        <RecipeIngredientsEdit/>
        <RecipeStepsEdit/>
    </div>
</template>
<script setup lang="ts">
import RecipeHeaderEdit from '../components/recipeHeaderEdit.vue';
import RecipeIngredientsEdit from '../components/recipeIngredientsEdit.vue';
import RecipeStepsEdit from '../components/recipeStepsEdit.vue';

</script>
<style lang="scss">
    .recipe-editor-container
    {
        max-width: 1200px;
        margin: 0 auto;
    }
</style>