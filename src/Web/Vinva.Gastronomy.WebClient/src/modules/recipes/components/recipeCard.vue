<template>
    <Card>
        <template #header>
            <img alt="recipe img" src="/logo.svg" />
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
    
</style>