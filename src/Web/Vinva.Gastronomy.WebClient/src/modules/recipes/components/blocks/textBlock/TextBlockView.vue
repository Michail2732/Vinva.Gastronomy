<template>
    <NodeViewWrapper class="recipe-block text-block">
        <BlockDragHandle v-if="isEditable" :editor="editor" class="text-block__handle" />
        <RecipeNodeToolbar :editable="isEditable" :delete-node="deleteNode" delete-label="Удалить блок" class="text-block__toolbar" />
        <NodeViewContent as="p" class="text-block__content" :style="{ textAlign: node.attrs.textAlign || undefined }" />
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
    node: { attrs: { textAlign?: string | null } }
    deleteNode: () => void
}>()
</script>

<style scoped lang="scss">
.text-block { margin: 0 0 1.5rem; position: relative; display: flex; align-items: center; min-height: 1.7em; }
.text-block__content { color: var(--p-text-color); font-size: 1rem; line-height: 1.7; margin: 0; min-height: 1.7em; outline: none; flex: 1; }
.text-block__content > * { margin: 0; }
.text-block__handle { top: 50% !important; transform: translateY(-50%); }
.text-block__toolbar { top: 50% !important; transform: translateY(-50%); }
</style>
