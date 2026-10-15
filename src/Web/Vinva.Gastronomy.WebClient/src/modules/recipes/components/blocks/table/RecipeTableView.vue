<template>
    <NodeViewWrapper as="div" class="tableWrapper recipe-table recipe-block">
        <BlockDragHandle v-if="isEditable" :editor="editor" />
        <RecipeTableToolbar :editor="editor" />
        <table class="recipe-table__element" :style="tableStyle">
            <colgroup v-html="colgroupHtml" />
            <NodeViewContent as="tbody" />
        </table>
    </NodeViewWrapper>
</template>

<script setup lang="ts">
import { computed, inject, ref } from 'vue'
import type { Ref } from 'vue'
const isEditable = inject<Ref<boolean>>('editorEditable', ref(true))
import type { Editor } from '@tiptap/core'
import type { Node as ProseMirrorNode } from '@tiptap/pm/model'
import { NodeViewContent, NodeViewWrapper } from '@tiptap/vue-3'
import RecipeTableToolbar from './RecipeTableToolbar.vue'
import BlockDragHandle from '../drag/BlockDragHandle.vue'

type TableColumn = { width?: number; minWidth: number }

const props = defineProps<{
    node: ProseMirrorNode
    editor: Editor
}>()

const defaultCellMinWidth = 80

const layout = computed(() => {
    const columns: TableColumn[] = []
    let totalWidth = 0
    let fixedWidth = true

    const row = props.node.firstChild
    if (row) {
        for (let index = 0; index < row.childCount; index++) {
            const cell = row.child(index)
            const colspan = Number(cell.attrs.colspan ?? 1) || 1
            const colwidth = (cell.attrs.colwidth ?? null) as (number | null)[] | null
            for (let span = 0; span < colspan; span++) {
                const hasWidth = Number(colwidth?.[span] ?? 0)
                totalWidth += hasWidth || defaultCellMinWidth
                if (!hasWidth) fixedWidth = false
                columns.push(hasWidth
                    ? { width: Math.max(hasWidth, defaultCellMinWidth), minWidth: defaultCellMinWidth }
                    : { minWidth: defaultCellMinWidth })
            }
        }
    }

    const styleAttribute = props.node.attrs.style
    const hasUserWidth = typeof styleAttribute === 'string' && /\bwidth\s*:/i.test(styleAttribute)
    const fixed = fixedWidth && !hasUserWidth

    return {
        columns,
        width: fixed ? `${totalWidth}px` : '',
        minWidth: fixed ? '' : `${totalWidth}px`,
    }
})

const tableStyle = computed(() => {
    const attrs = props.node.attrs
    return {
        '--recipe-grid-color': attrs.gridColor || '#d1d5db',
        '--recipe-grid-width': `${Math.min(20, Math.max(0, Number(attrs.gridWidth) || 0))}px`,
        ...(attrs.backgroundColor ? { '--recipe-table-background': String(attrs.backgroundColor) } : {}),
        width: layout.value.width,
        minWidth: layout.value.minWidth,
    }
})

const colgroupHtml = computed(() => layout.value.columns
    .map((column) => `<col style="${column.width ? `width: ${column.width}px` : `min-width: ${column.minWidth}px`}">`)
    .join(''))
</script>

<style scoped lang="scss">
.tableWrapper.recipe-table { position: relative; }
.recipe-table__element :deep(td),
.recipe-table__element :deep(th) { white-space: normal; }
</style>


