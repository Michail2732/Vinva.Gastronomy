import { Table, TableCell, TableHeader, TableRow } from '@tiptap/extension-table'
import { VueNodeViewRenderer } from '@tiptap/vue-3'
import type { NodeViewProps } from '@tiptap/core'
import type { Component } from 'vue'
import { CellSelection, TableMap } from '@tiptap/pm/tables'
import type { Editor } from '@tiptap/core'
import { NodeSelection, TextSelection } from '@tiptap/pm/state'
import RecipeTableView from './RecipeTableView.vue'
import { selectCellScope } from './selectCellScope'

function tableStyleAttributes() {
    return {
        backgroundColor: {
            default: null,
            parseHTML: (element: HTMLElement) => element.style.getPropertyValue('--recipe-table-background') || null,
            renderHTML: (attributes: { backgroundColor?: string | null }) => attributes.backgroundColor ? { style: `--recipe-table-background: ${attributes.backgroundColor};` } : {},
        },
        gridColor: {
            default: '#d1d5db',
            parseHTML: (element: HTMLElement) => element.style.getPropertyValue('--recipe-grid-color') || '#d1d5db',
            renderHTML: (attributes: { gridColor?: string }) => ({ style: `--recipe-grid-color: ${attributes.gridColor || '#d1d5db'};` }),
        },
        gridWidth: {
            default: 1,
            parseHTML: (element: HTMLElement) => Number.parseFloat(element.style.getPropertyValue('--recipe-grid-width')) || 1,
            renderHTML: (attributes: { gridWidth?: number }) => ({ style: `--recipe-grid-width: ${Math.min(20, Math.max(0, Number(attributes.gridWidth) || 0))}px;` }),
        },
    }
}

function cellStyleAttributes() {
    return {
        backgroundColor: {
            default: null,
            parseHTML: (element: HTMLElement) => element.style.backgroundColor || null,
            renderHTML: (attributes: { backgroundColor?: string | null }) => attributes.backgroundColor ? { style: `background-color: ${attributes.backgroundColor};` } : {},
        },
        borderColor: {
            default: null,
            parseHTML: (element: HTMLElement) => {
                const raw = element.style.getPropertyValue('--recipe-cell-border-color') || element.style.borderTopColor
                return raw || null
            },
            renderHTML: (attributes: { borderColor?: string | null }) => attributes.borderColor
                ? { class: 'has-cell-border', style: `--recipe-cell-border-color: ${attributes.borderColor};` }
                : {},
        },
        borderTopWidth: cellBorderWidthAttribute('top'),
        borderRightWidth: cellBorderWidthAttribute('right'),
        borderBottomWidth: cellBorderWidthAttribute('bottom'),
        borderLeftWidth: cellBorderWidthAttribute('left'),
    }
}

function cellBorderWidthAttribute(side: 'top' | 'right' | 'bottom' | 'left') {
    const field = `border${side[0]!.toUpperCase()}${side.slice(1)}Width`
    return {
        default: null as number | null,
        parseHTML: (element: HTMLElement) => {
            const raw = element.style.getPropertyValue(`--recipe-cell-border-${side}`) || element.style[`border${side[0]!.toUpperCase()}${side.slice(1)}Width` as 'borderTopWidth']
            const parsed = Number.parseFloat(raw)
            return Number.isFinite(parsed) ? parsed : null
        },
        renderHTML: (attributes: Record<string, number | null>) => {
            const width = Math.min(20, Math.max(0, Number(attributes[field]) || 0))
            return width > 0
                ? { class: 'has-cell-border', style: `--recipe-cell-border-${side}: ${width}px;` }
                : {}
        },
    }
}

export const RecipeTable = Table.extend({
    addAttributes() {
        return { ...this.parent?.(), ...tableStyleAttributes() }
    },
    addNodeView: () => VueNodeViewRenderer(RecipeTableView as unknown as Component),
    addKeyboardShortcuts() {
        return {
            'Mod-a': ({ editor }) => selectCellScope(editor),
        }
    },
}).configure({ resizable: false })

export const RecipeTableRow = TableRow
export const RecipeTableHeader = TableHeader.extend({
    addAttributes() {
        return { ...this.parent?.(), ...cellStyleAttributes() }
    },
})

export const RecipeTableCell = TableCell.extend({
    addAttributes() {
        return { ...this.parent?.(), ...cellStyleAttributes() }
    },
})

