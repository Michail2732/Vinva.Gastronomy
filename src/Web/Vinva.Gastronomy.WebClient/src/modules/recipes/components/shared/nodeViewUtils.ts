import type { EditorView } from '@tiptap/pm/view'
import type { Component } from 'vue'
import { NodeSelection } from '@tiptap/pm/state'

export function getCellPosition(view: EditorView, target: EventTarget | null) {
    if (!(target instanceof Element)) return null
    const cell = target.closest('td, th')
    if (!cell || !view.dom.contains(cell)) return null

    const position = view.posAtDOM(cell, 0)
    if (position < 0) return null
    const resolved = view.state.doc.resolve(position)
    for (let depth = resolved.depth; depth > 0; depth--) {
        const node = resolved.node(depth)
        if (node.type.name === 'tableCell' || node.type.name === 'tableHeader') return resolved.before(depth)
    }
    return null
}

export function findBlockPosByElement(view: EditorView, element: HTMLElement): number | null {
    let found: number | null = null
    view.state.doc.descendants((_node, pos) => {
        if (view.nodeDOM(pos) === element) {
            found = pos
            return false
        }
        return true
    })
    return found
}

export function getActiveBlockPos(doc: import('@tiptap/pm/model').Node, selection: import('@tiptap/pm/state').Selection): number | null {
    const isBlockNode = (node: { type: { name: string } }) => {
        const name = node.type.name
        return name === 'paragraph' || name === 'heading' || name === 'textBlock' || name === 'groupBox' || name === 'ingredientList'
            || name === 'table' || name === 'recipeDivider' || name === 'horizontalRule' || name === 'blockquote'
            || name === 'codeBlock' || name === 'taskList' || name === 'imageGallery' || name === 'recipeImage'
            || name === 'recipeMedia' || name === 'recipeToggle'
    }
    if (selection instanceof NodeSelection) {
        const node = selection.node
        if (node && isBlockNode(node)) return selection.from
    }
    const $from = selection.$from
    if ($from.depth >= 1 && isBlockNode($from.node(1))) {
        return $from.before(1)
    }
    return null
}
