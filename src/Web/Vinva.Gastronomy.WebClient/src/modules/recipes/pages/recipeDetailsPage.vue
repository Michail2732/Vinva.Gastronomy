<template>
    <RecipeDocumentView
        :recipe-id="route.params.id as string"
        :title="recipe?.name ?? 'Рецепт'"
        :description="recipe?.description"
        :document="document"
    />
</template>

<script setup lang="ts">
import { computed, ref, watch } from 'vue'
import { useRoute } from 'vue-router'
import { useToast } from 'primevue'
import RecipeDocumentView from '../components/recipeDocumentView.vue'
import { useRecipesStore } from '../stores/recipesStore'
import { createRecipeDocument } from '../recipeDocumentModel'
import type { RecipeDetailsViewModel } from '../types/recipeTypes'

const route = useRoute()
const toast = useToast()
const recipesStore = useRecipesStore()
const recipe = ref<RecipeDetailsViewModel | null>(null)
const document = computed(() => createRecipeDocument(recipe.value))

async function loadRecipe(id: string) {
    const result = await recipesStore.getRecipeById(id)
    if (!result.isSuccess) {
        toast.add({ severity: 'error', summary: 'Ошибка', detail: result.error, life: 3500 })
        return
    }
    recipe.value = result.data
}

watch(
    () => route.params.id,
    (id) => {
        if (typeof id === 'string') loadRecipe(id)
    },
    { immediate: true },
)
</script>
