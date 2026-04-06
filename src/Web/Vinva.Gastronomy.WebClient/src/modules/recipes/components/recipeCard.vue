<template>
    <Card class="card">
        <template #header>
            <img alt="recipe img" src="/logo.svg" class="card-recipe-img" />
        </template>
        <template #title>{{ cardVm?.name}}</template>
    </Card>
</template>
<script setup lang="ts">
import {ref} from 'vue'
import type { RecipeCardViewModel } from '../types/recipeTypes'

const cardVm = defineModel<RecipeCardViewModel>()
</script>
<style scoped lang="scss">
    .card
    {        
        overflow: hidden;
        .card-recipe-img
        {
            width: 100%;
            max-width: 100%;
            height: auto;
            display: block;
        }
    }
</style>