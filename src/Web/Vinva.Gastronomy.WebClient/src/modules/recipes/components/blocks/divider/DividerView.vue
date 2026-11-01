<template>
    <NodeViewWrapper class="recipe-block recipe-divider-block" :style="spacingStyle">
        <BlockDragHandle v-if="isEditable" :editor="editor" />
        <RecipeNodeToolbar :editable="isEditable" :delete-node="deleteNode" delete-label="Удалить разделитель">
            <RecipeBlockSpacing :attrs="node.attrs" :update-attributes="updateAttributes" />
        </RecipeNodeToolbar>
        <hr />
    </NodeViewWrapper>
</template>

<script setup lang="ts">
import { computed, inject, ref } from 'vue'
import type { Ref } from 'vue'
import type { Editor } from '@tiptap/core'
import { NodeViewWrapper } from '@tiptap/vue-3'
import RecipeNodeToolbar from '../../editor/recipeNodeToolbar.vue'
import RecipeBlockSpacing from '../../editor/recipeBlockSpacing.vue'
import { blockMarginStyle } from '../../shared/margins'
import BlockDragHandle from '../drag/BlockDragHandle.vue'

const props = defineProps<{
    editor: Editor
    node: { attrs: Record<string, unknown> }
    updateAttributes: (attributes: Record<string, unknown>) => void
    deleteNode: () => void
}>()

const isEditable = inject<Ref<boolean>>('editorEditable', ref(true))
const spacingStyle = computed(() => blockMarginStyle(props.node.attrs))
</script>

<style scoped lang="scss">
.recipe-divider-block { margin: 1.5rem 0; }
hr { border: 0; border-top: 2px solid var(--p-primary-color); margin: 0; opacity: .6; }
</style>
