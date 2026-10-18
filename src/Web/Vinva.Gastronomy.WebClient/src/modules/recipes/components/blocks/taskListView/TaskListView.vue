<template>
    <NodeViewWrapper class="recipe-block task-list-block">
        <BlockDragHandle v-if="isEditable" :editor="editor" />
        <RecipeNodeToolbar :editable="isEditable" :delete-node="deleteNode" delete-label="Удалить блок" />
        <NodeViewContent as="ul" class="task-list-content" />
    </NodeViewWrapper>
</template>

<script setup lang="ts">
import { inject, ref } from 'vue'
import type { Ref } from 'vue'
const isEditable = inject<Ref<boolean>>('editorEditable', ref(true))
import type { Editor } from '@tiptap/core'
import { NodeViewContent, NodeViewWrapper } from '@tiptap/vue-3'
import RecipeNodeToolbar from '../../editor/recipeNodeToolbar.vue'
import BlockDragHandle from '../drag/BlockDragHandle.vue'

defineProps<{
    editor: Editor
    deleteNode: () => void
}>()
</script>

<style scoped lang="scss">
.task-list-block { margin: 1.25rem 0; position: relative; }
.task-list-content { margin: 0; padding-left: 1.5rem; }
.task-list-content :deep(li) { margin: .35rem 0; }
.task-list-content :deep(li p) { margin: 0; }
</style>
