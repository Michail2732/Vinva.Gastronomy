<template>
    <NodeViewWrapper class="recipe-block recipe-divider-block">
        <RecipeNodeToolbar :editable="editor.isEditable" :delete-node="deleteNode" delete-label="Удалить разделитель" />
        <hr />
    </NodeViewWrapper>
</template>

<script setup lang="ts">
import { NodeViewWrapper } from '@tiptap/vue-3'
import RecipeNodeToolbar from './recipeNodeToolbar.vue'

const props = defineProps<{ editor: { isEditable: boolean }; deleteNode: () => void }>()
</script>

<style scoped lang="scss">
.recipe-divider-block { position: relative; }
.recipe-divider-block hr { border: 0; border-top: 1px solid var(--p-content-border-color); margin: 2rem 0; }
</style>
