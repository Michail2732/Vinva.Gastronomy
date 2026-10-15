<template>
    <div v-if="editable" class="table-local-toolbar" :class="{ 'is-popover-open': cellPopoverVisible }" role="toolbar" aria-label="Форматирование таблицы">
        <Button icon="pi pi-plus" text severity="secondary" v-tooltip.top="'Добавить столбец'" aria-label="Добавить столбец" :disabled="!canAddColumn" @mousedown.prevent @click="editor.chain().focus().addColumnAfter().run()" class="table-toolbar__rotate-90" />
        <Button icon="pi pi-plus" text severity="secondary" v-tooltip.top="'Добавить строку'" aria-label="Добавить строку" :disabled="!canAddRow" @mousedown.prevent @click="editor.chain().focus().addRowAfter().run()" />
        <Button icon="pi pi-sliders-h" text severity="secondary" v-tooltip.top="'Настроить ячейки'" aria-label="Настройка ячеек" :disabled="!hasCellSelection" @mousedown.prevent @click="toggleCellPopover" />
        <Button icon="pi pi-minus" text severity="secondary" v-tooltip.top="'Удалить столбец'" aria-label="Удалить столбец" :disabled="!canDeleteColumn" @mousedown.prevent @click="editor.chain().focus().deleteColumn().run()" class="table-toolbar__rotate-90" />
        <Button icon="pi pi-minus" text severity="secondary" v-tooltip.top="'Удалить строку'" aria-label="Удалить строку" :disabled="!canDeleteRow" @mousedown.prevent @click="editor.chain().focus().deleteRow().run()" />
        <Button icon="pi pi-trash" text severity="danger" class="table-toolbar__delete" v-tooltip.top="'Удалить таблицу'" aria-label="Удалить таблицу" :disabled="!canDeleteTable" @mousedown.prevent @click="editor.chain().focus().deleteTable().run()" />
        <Popover ref="cellPopover" @show="cellPopoverVisible = true" @hide="cellPopoverVisible = false">
            <div class="table-cell-settings" aria-label="Настройки выделенных ячеек">
                <label class="table-cell-settings__row">
                    <span>Заливка</span>
                    <input type="color" aria-label="Заливка выделенных ячеек" :value="tableToolbarState.cellBackgroundColor || '#ffffff'" @input="setCellAttribute('backgroundColor', $event)" />
                </label>
                <label class="table-cell-settings__row">
                    <span>Цвет границы</span>
                    <input type="color" aria-label="Цвет границ выделенных ячеек" :value="tableToolbarState.cellBorderColor || tableToolbarState.gridColor || '#d1d5db'" @input="setCellAttribute('borderColor', $event)" />
                </label>
                <div class="table-cell-settings__section">
                    <span class="table-cell-settings__section-title">Толщина границы</span>
                    <div class="border-side-grid">
                        <label class="table-cell-settings__row table-cell-settings__row--side">
                            <span>Верх</span>
                            <InputNumber :model-value="borderWidthState.top" :min="0" :max="20" :use-grouping="false" suffix=" px" :placeholder="String(tableToolbarState.gridWidth ?? 1)" aria-label="Верхняя граница выделенных ячеек" @update:model-value="setBorderSide('top', $event ?? null)" />
                        </label>
                        <label class="table-cell-settings__row table-cell-settings__row--side">
                            <span>Низ</span>
                            <InputNumber :model-value="borderWidthState.bottom" :min="0" :max="20" :use-grouping="false" suffix=" px" :placeholder="String(tableToolbarState.gridWidth ?? 1)" aria-label="Нижняя граница выделенных ячеек" @update:model-value="setBorderSide('bottom', $event ?? null)" />
                        </label>
                        <label class="table-cell-settings__row table-cell-settings__row--side">
                            <span>Лево</span>
                            <InputNumber :model-value="borderWidthState.left" :min="0" :max="20" :use-grouping="false" suffix=" px" :placeholder="String(tableToolbarState.gridWidth ?? 1)" aria-label="Левая граница выделенных ячеек" @update:model-value="setBorderSide('left', $event ?? null)" />
                        </label>
                        <label class="table-cell-settings__row table-cell-settings__row--side">
                            <span>Право</span>
                            <InputNumber :model-value="borderWidthState.right" :min="0" :max="20" :use-grouping="false" suffix=" px" :placeholder="String(tableToolbarState.gridWidth ?? 1)" aria-label="Правая граница выделенных ячеек" @update:model-value="setBorderSide('right', $event ?? null)" />
                        </label>
                    </div>
                </div>
            </div>
        </Popover>
    </div>
</template>

<script setup lang="ts">
import { computed, onBeforeUnmount, onMounted, ref, watch } from 'vue'
import type { Editor } from '@tiptap/core'
import { CellSelection, selectedRect } from '@tiptap/pm/tables'
import Popover from 'primevue/popover'

type BorderSide = 'top' | 'right' | 'bottom' | 'left'

function borderField(side: BorderSide) {
    return `border${side.charAt(0).toUpperCase()}${side.slice(1)}Width` as const
}

const props = defineProps<{
    editor: Editor
}>()

const revision = ref(0)
const cellPopover = ref<{ toggle: (event: Event) => void; hide: () => void } | null>(null)
const cellPopoverVisible = ref(false)

const editable = computed(() => {
    revision.value
    return props.editor.isEditable
})

const hasCellSelection = computed(() => {
    revision.value
    const sel = props.editor.state.selection
    if (sel instanceof CellSelection) return true
    const { cellPosition } = getActiveTablePositions()
    return cellPosition !== null
})

const canAddColumn = computed(() => {
    revision.value
    return props.editor.can().addColumnAfter()
})

const canAddRow = computed(() => {
    revision.value
    return props.editor.can().addRowAfter()
})

const canDeleteColumn = computed(() => {
    revision.value
    return props.editor.can().deleteColumn()
})

const canDeleteRow = computed(() => {
    revision.value
    return props.editor.can().deleteRow()
})

const canDeleteTable = computed(() => {
    revision.value
    return props.editor.can().deleteTable()
})

const tableToolbarState = computed(() => {
    revision.value
    const { tablePosition, cellPosition } = getActiveTablePositions()
    const tableNode = tablePosition === null ? null : props.editor.state.doc.nodeAt(tablePosition)
    const cellNode = cellPosition === null ? null : props.editor.state.doc.nodeAt(cellPosition)
    return {
        gridColor: tableNode?.attrs.gridColor as string | null,
        gridWidth: tableNode?.attrs.gridWidth as number | null,
        cellBackgroundColor: cellPosition === null ? undefined : cellNode?.attrs.backgroundColor as string | null,
        cellBorderColor: cellPosition === null ? undefined : cellNode?.attrs.borderColor as string | null,
    }
})

const borderWidthState = computed(() => {
    revision.value
    const rect = getRegionRect()
    if (!rect) return { top: null, right: null, bottom: null, left: null }
    return {
        top: getSideBaseValue(rect, 'top'),
        right: getSideBaseValue(rect, 'right'),
        bottom: getSideBaseValue(rect, 'bottom'),
        left: getSideBaseValue(rect, 'left'),
    }
})

watch(hasCellSelection, (selected) => {
    if (!selected) cellPopover.value?.hide()
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

function toggleCellPopover(event: Event) {
    cellPopover.value?.toggle(event)
}

function refreshState() {
    revision.value++
}

function applyToCells(update: (attrs: Record<string, unknown>) => Record<string, unknown>) {
    const { state, view } = props.editor
    const tr = state.tr
    let changed = false
    const mark = (pos: number, node: { attrs: Record<string, unknown> }) => {
        const next = update({ ...node.attrs })
        if (Object.keys(next).every((key) => next[key] === node.attrs[key])) return
        tr.setNodeMarkup(pos, undefined, next)
        changed = true
    }
    if (state.selection instanceof CellSelection) {
        state.selection.forEachCell((node, pos) => mark(pos, node))
    } else {
        const { cellPosition } = getActiveTablePositions()
        const cellNode = cellPosition === null ? null : state.doc.nodeAt(cellPosition)
        if (cellNode && cellPosition !== null) mark(cellPosition, cellNode)
    }
    if (changed) view.dispatch(tr)
}

function setCellAttribute(field: 'backgroundColor' | 'borderColor', event: Event) {
    const value = (event.target as HTMLInputElement).value
    applyToCells((attrs) => ({ ...attrs, [field]: value }))
}

function getRegionRect() {
    const { state } = props.editor
    if (!(state.selection instanceof CellSelection)) return null
    try {
        return selectedRect(state)
    } catch {
        return null
    }
}

function getSideCellOffsets(rect: ReturnType<typeof selectedRect>, side: BorderSide) {
    switch (side) {
        case 'top': return rect.map.cellsInRect({ left: rect.left, top: rect.top, right: rect.right, bottom: rect.top + 1 })
        case 'bottom': return rect.map.cellsInRect({ left: rect.left, top: rect.bottom - 1, right: rect.right, bottom: rect.bottom })
        case 'left': return rect.map.cellsInRect({ left: rect.left, top: rect.top, right: rect.left + 1, bottom: rect.bottom })
        case 'right': return rect.map.cellsInRect({ left: rect.right - 1, top: rect.top, right: rect.right, bottom: rect.bottom })
    }
}

function getSideBaseValue(rect: ReturnType<typeof selectedRect>, side: BorderSide) {
    const field = borderField(side)
    for (const offset of getSideCellOffsets(rect, side)) {
        const node = props.editor.state.doc.nodeAt(rect.tableStart + offset)
        if (!node) continue
        const raw = node.attrs[field]
        if (raw != null) return raw as number
    }
    return null
}

function setBorderSide(side: BorderSide, value: number | null) {
    const rect = getRegionRect()
    if (!rect) return
    const { state, view } = props.editor
    const tr = state.tr
    const field = borderField(side)
    const nextValue = value === null ? null : Math.min(20, Math.max(0, value))
    let changed = false
    for (const offset of getSideCellOffsets(rect, side)) {
        const pos = rect.tableStart + offset
        const node = state.doc.nodeAt(pos)
        if (!node) continue
        if (node.attrs[field] === nextValue) continue
        tr.setNodeMarkup(pos, undefined, { ...node.attrs, [field]: nextValue })
        changed = true
    }
    if (changed) view.dispatch(tr)
}

onMounted(() => {
    props.editor.on('update', refreshState)
    props.editor.on('selectionUpdate', refreshState)
    refreshState()
})

onBeforeUnmount(() => {
    props.editor.off('update', refreshState)
    props.editor.off('selectionUpdate', refreshState)
})
</script>

<style scoped lang="scss">
.table-local-toolbar { align-items: center; background: var(--p-surface-0); border: 1px solid var(--p-content-border-color); border-radius: 4px; box-shadow: var(--p-overlay-popover-shadow); display: inline-flex; gap: .15rem; opacity: 0; padding: .15rem; pointer-events: none; position: absolute; right: .4rem; top: .35rem; transition: opacity var(--p-transition-duration); z-index: 3; }
:global(.tableWrapper.recipe-table:hover > .table-local-toolbar), :global(.tableWrapper.recipe-table:focus-within > .table-local-toolbar), .table-local-toolbar.is-popover-open { opacity: 1; pointer-events: auto; }
:global(.paper-shell.is-preview .table-local-toolbar) { display: none; }
.table-local-toolbar :deep(.p-button) { flex: 0 0 auto; height: 1.65rem; padding: 0; width: 1.65rem; }
.table-toolbar__delete:hover { background: var(--p-red-50); }
.table-toolbar__rotate-90 :deep(.pi) { transform: rotate(90deg); }
.table-cell-settings { display: grid; gap: .55rem; min-width: 13rem; }
.table-cell-settings__row { align-items: center; color: var(--p-text-muted-color); display: flex; font-size: .75rem; gap: .75rem; justify-content: space-between; }
.table-cell-settings__row input[type='color'] { border: 1px solid var(--p-content-border-color); border-radius: 3px; cursor: pointer; height: 1.7rem; padding: .1rem; width: 2.4rem; }
.table-cell-settings__section { display: grid; gap: .45rem; }
.table-cell-settings__section-title { color: var(--p-text-muted-color); font-size: .7rem; font-weight: 700; text-transform: uppercase; }
.border-side-grid { display: grid; gap: .45rem .75rem; grid-template-columns: 1fr 1fr; }
.table-cell-settings__row--side { gap: .35rem; }
.table-cell-settings__row--side :deep(.p-inputnumber) { flex: 0 0 4.2rem; width: 4.2rem; }
.table-cell-settings__row--side :deep(.p-inputnumber-input) { font-size: .72rem; min-width: 0; padding: .3rem .15rem; text-align: center; }
</style>
