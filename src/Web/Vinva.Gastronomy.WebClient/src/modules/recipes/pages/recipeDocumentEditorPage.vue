<template>
    <RecipeDocumentEditor :initial-title="title" :initial-document="document" />
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { useRoute } from 'vue-router'
import RecipeDocumentEditor from '../components/recipeDocumentEditor.vue'
import { sampleRecipeDocument } from '../recipeDocumentModel'

const route = useRoute()
const title = computed(() => route.params.id ? `Редактирование рецепта ${route.params.id}` : 'Новый рецепт')
const document = sampleRecipeDocument
</script>
