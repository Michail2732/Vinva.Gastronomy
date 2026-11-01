<template>
    <NodeViewWrapper ref="wrapper" class="recipe-block text-block" :class="{ 'is-multi-line': !isSingleLine }" :style="spacingStyle">
        <BlockDragHandle v-if="isEditable" :editor="editor" class="text-block__handle" />
        <RecipeNodeToolbar :editable="isEditable" :delete-node="deleteNode" delete-label="Удалить блок" class="text-block__toolbar">
            <RecipeBlockSpacing :attrs="node.attrs" :update-attributes="updateAttributes" />
        </RecipeNodeToolbar>
        <NodeViewContent as="p" class="text-block__content" :style="{ textAlign: node.attrs.textAlign || undefined }" />
    </NodeViewWrapper>
</template>

<script setup lang="ts">
import { computed, inject, onBeforeUnmount, onMounted, ref } from 'vue'
import type { Ref } from 'vue'
const isEditable = inject<Ref<boolean>>('editorEditable', ref(true))
import type { Editor } from '@tiptap/core'
import { NodeViewContent, NodeViewWrapper } from '@tiptap/vue-3'
import RecipeNodeToolbar from '../../editor/recipeNodeToolbar.vue'
import RecipeBlockSpacing from '../../editor/recipeBlockSpacing.vue'
import { blockMarginStyle } from '../../shared/margins'
import BlockDragHandle from '../drag/BlockDragHandle.vue'

const props = defineProps<{
    editor: Editor
    node: { attrs: { textAlign?: string | null } }
    updateAttributes: (attributes: Record<string, unknown>) => void
    deleteNode: () => void
}>()

const spacingStyle = computed(() => blockMarginStyle(props.node.attrs as unknown as Record<string, unknown>))
const isSingleLine = ref(true)
const wrapper = ref<{ $el?: HTMLElement } | null>(null)

let lineObserver: ResizeObserver | null = null

function measureLineCount() {
    const content = wrapper.value?.$el?.querySelector<HTMLElement>('.text-block__content') ?? null
    if (!content) return
    const computedStyle = getComputedStyle(content)
    const fontSize = Number.parseFloat(computedStyle.fontSize) || 16
    const lineHeight = Number.parseFloat(computedStyle.lineHeight) || fontSize * 1.7
    isSingleLine.value = content.clientHeight <= lineHeight + 2
}

onMounted(() => {
    const element = wrapper.value?.$el ?? null
    if (element) {
        lineObserver = new ResizeObserver(() => measureLineCount())
        lineObserver.observe(element)
    }
    measureLineCount()
})

onBeforeUnmount(() => {
    lineObserver?.disconnect()
})
</script>

<style scoped lang="scss">
.text-block { margin: 0 0 1.5rem; position: relative; display: flex; align-items: center; min-height: 1.7em; }
.text-block__content { color: var(--p-text-color); font-size: 1rem; line-height: 1.7; margin: 0; min-height: 1.7em; outline: none; flex: 1; }
.text-block__content > * { margin: 0; }
.text-block__handle { top: 50% !important; transform: translateY(-50%); }
.text-block__toolbar { top: 50% !important; transform: translateY(-50%); }
.text-block.is-multi-line .text-block__handle { top: 6px !important; transform: none; }
.text-block.is-multi-line .text-block__toolbar { top: .35rem !important; transform: none; }
</style>
