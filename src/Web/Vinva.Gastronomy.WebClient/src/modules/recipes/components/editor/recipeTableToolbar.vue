<template>
    <div v-if="tableToolbarVisible" ref="toolbarElement" class="table-local-toolbar" role="toolbar" aria-label="Форматирование таблицы" :style="toolbarPosition">
        <span class="table-toolbar-title"><i class="pi pi-table" /> Таблица</span>
        <Button icon="pi pi-plus" text severity="secondary" size="small" v-tooltip.top="'Добавить столбец слева'" aria-label="Добавить столбец слева" @mousedown.prevent @click="editor.chain().focus().addColumnBefore().run()" />
        <Button icon="pi pi-arrow-right" text severity="secondary" size="small" v-tooltip.top="'Добавить столбец справа'" aria-label="Добавить столбец справа" @mousedown.prevent @click="editor.chain().focus().addColumnAfter().run()" />
        <Button icon="pi pi-minus" text severity="secondary" size="small" v-tooltip.top="'Удалить столбец'" aria-label="Удалить столбец" :disabled="!editor.can().deleteColumn()" @mousedown.prevent @click="editor.chain().focus().deleteColumn().run()" />
        <label title="Фон таблицы"><i class="pi pi-palette" /><input type="color" aria-label="Фон таблицы" :value="tableToolbarState.backgroundColor || '#ffffff'" @input="setTableAttribute('backgroundColor', $event)" /></label>
        <label title="Цвет сетки"><i class="pi pi-th-large" /><input type="color" aria-label="Цвет сетки таблицы" :value="tableToolbarState.gridColor || '#d1d5db'" @input="setTableAttribute('gridColor', $event)" /></label>
        <label class="grid-width-control" title="Толщина сетки"><i class="pi pi-arrows-h" /><InputNumber :model-value="tableToolbarState.gridWidth ?? 1" :min="0" :max="20" :use-grouping="false" suffix=" px" aria-label="Толщина сетки" @update:model-value="setTableNumberAttribute('gridWidth', $event)" /></label>
        <span class="table-toolbar-divider" />
        <label title="Фон ячейки"><i class="pi pi-stop" /><input type="color" aria-label="Фон выбранной ячейки" :disabled="tableToolbarState.cellBackgroundColor === undefined" :value="tableToolbarState.cellBackgroundColor || '#ffffff'" @input="setCellBackground($event)" /></label>
    </div>
</template>

<script setup lang="ts">
import { computed, onBeforeUnmount, onMounted, ref, watch } from 'vue'
import type { Editor } from '@tiptap/core'

const props = defineProps<{
    editor: Editor
}>()

const revision = ref(0)
const toolbarElement = ref<HTMLElement | null>(null)
const toolbarPosition = ref<Record<string, string>>({})

const tableToolbarVisible = computed(() => {
    revision.value
    return props.editor.isEditable && props.editor.isActive('table')
})

const tableToolbarState = computed(() => {
    revision.value
    const { tablePosition, cellPosition } = getActiveTablePositions()
    const tableNode = tablePosition === null ? null : props.editor.state.doc.nodeAt(tablePosition)
    const cellNode = cellPosition === null ? null : props.editor.state.doc.nodeAt(cellPosition)
    return {
        backgroundColor: tableNode?.attrs.backgroundColor as string | null,
        gridColor: tableNode?.attrs.gridColor as string | null,
        gridWidth: tableNode?.attrs.gridWidth as number | null,
        cellBackgroundColor: cellPosition === null ? undefined : cellNode?.attrs.backgroundColor as string | null,
    }
})

watch(tableToolbarVisible, async (visible) => {
    if (!visible) return
    await new Promise<void>((resolve) => requestAnimationFrame(() => resolve()))
    updateToolbarPosition()
})

function getActiveTablePositions() {
    const { $from } = props.editor.state.selection
    let tablePosition: number | null = null
    let cellPosition: number | null = null
    for (let depth = $from.depth; depth > 0; depth--) {
        const node = $from.node(depth)
        if (node.type.name === 'table') tablePosition = $from.before(depth)
        if (node.type.name === 'tableCell' || node.type.name === 'tableHeader') cellPosition = $from.before(depth)
    }
    return { tablePosition, cellPosition }
}

function updateActiveTableStyles() {
    const { tablePosition } = getActiveTablePositions()
    if (tablePosition === null) return
    const nodeElement = props.editor.view.nodeDOM(tablePosition)
    const tableElement = nodeElement instanceof HTMLElement
        ? nodeElement.matches('table') ? nodeElement : nodeElement.querySelector('table')
        : null
    const tableNode = props.editor.state.doc.nodeAt(tablePosition)
    if (!(tableElement instanceof HTMLElement) || !tableNode) return
    tableElement.style.setProperty('--recipe-table-background', tableNode.attrs.backgroundColor || 'transparent')
    tableElement.style.setProperty('--recipe-grid-color', tableNode.attrs.gridColor || '#d1d5db')
    tableElement.style.setProperty('--recipe-grid-width', `${Math.min(20, Math.max(0, Number(tableNode.attrs.gridWidth) || 0))}px`)
}

function updateToolbarPosition() {
    if (!tableToolbarVisible.value || !toolbarElement.value) return
    const { tablePosition } = getActiveTablePositions()
    if (tablePosition === null) return
    const nodeElement = props.editor.view.nodeDOM(tablePosition)
    if (!(nodeElement instanceof HTMLElement)) return
    const tableElement = nodeElement.matches('table') ? nodeElement : nodeElement.querySelector('table')
    const target = tableElement?.closest('.tableWrapper') ?? tableElement
    if (!target) return
    const bounds = target.getBoundingClientRect()
    const panelWidth = toolbarElement.value.offsetWidth
    const left = Math.max(8, Math.min(window.innerWidth - panelWidth - 8, bounds.right - panelWidth))
    const top = Math.max(8, bounds.top - toolbarElement.value.offsetHeight - 8)
    toolbarPosition.value = { left: `${left}px`, top: `${top}px` }
}

function refreshTableState() {
    revision.value++
    updateActiveTableStyles()
    requestAnimationFrame(updateToolbarPosition)
}

function repositionToolbar() {
    requestAnimationFrame(updateToolbarPosition)
}

function setTableAttribute(field: 'backgroundColor' | 'gridColor', event: Event) {
    const { tablePosition } = getActiveTablePositions()
    const tableNode = tablePosition === null ? null : props.editor.state.doc.nodeAt(tablePosition)
    if (!tableNode || tablePosition === null) return
    props.editor.view.dispatch(props.editor.state.tr.setNodeMarkup(tablePosition, undefined, {
        ...tableNode.attrs,
        [field]: (event.target as HTMLInputElement).value,
    }))
}

function setTableNumberAttribute(field: 'gridWidth', value: number | null) {
    const { tablePosition } = getActiveTablePositions()
    const tableNode = tablePosition === null ? null : props.editor.state.doc.nodeAt(tablePosition)
    if (!tableNode || tablePosition === null || value === null) return
    props.editor.view.dispatch(props.editor.state.tr.setNodeMarkup(tablePosition, undefined, {
        ...tableNode.attrs,
        [field]: Math.min(20, Math.max(0, value)),
    }))
}

function setCellBackground(event: Event) {
    const { cellPosition } = getActiveTablePositions()
    const cellNode = cellPosition === null ? null : props.editor.state.doc.nodeAt(cellPosition)
    if (!cellNode || cellPosition === null) return
    props.editor.view.dispatch(props.editor.state.tr.setNodeMarkup(cellPosition, undefined, {
        ...cellNode.attrs,
        backgroundColor: (event.target as HTMLInputElement).value,
    }))
}

onMounted(() => {
    props.editor.on('update', refreshTableState)
    props.editor.on('selectionUpdate', refreshTableState)
    window.addEventListener('scroll', repositionToolbar, true)
    window.addEventListener('resize', repositionToolbar)
    refreshTableState()
})

onBeforeUnmount(() => {
    props.editor.off('update', refreshTableState)
    props.editor.off('selectionUpdate', refreshTableState)
    window.removeEventListener('scroll', repositionToolbar, true)
    window.removeEventListener('resize', repositionToolbar)
})
</script>

<style scoped lang="scss">
.table-local-toolbar { align-items: center; background: var(--p-surface-0); border: 1px solid var(--p-primary-200); border-radius: var(--p-border-radius-md, .5rem); box-shadow: var(--p-overlay-modal-shadow); display: flex; flex-wrap: wrap; gap: .2rem; left: 8px; max-width: min(42rem, calc(100vw - 16px)); padding: .3rem .5rem; position: fixed; top: 8px; z-index: 1000; }
.table-toolbar-title { color: var(--p-primary-700); font-size: .72rem; font-weight: 700; margin: 0 .3rem; }
.table-local-toolbar > label { align-items: center; color: var(--p-text-muted-color); display: inline-flex; gap: .25rem; padding: .15rem; position: relative; }
.table-local-toolbar input[type='color'] { cursor: pointer; height: 1.4rem; opacity: 0; position: absolute; width: 1.4rem; }
.table-local-toolbar label > i { font-size: .75rem; }
.table-toolbar-divider { background: var(--p-content-border-color); height: 1.35rem; margin: 0 .25rem; width: 1px; }
.grid-width-control :deep(.p-inputnumber) { width: 5rem; }
.grid-width-control :deep(.p-inputnumber-input) { font-size: .68rem; padding: .3rem .15rem; text-align: center; }
</style>