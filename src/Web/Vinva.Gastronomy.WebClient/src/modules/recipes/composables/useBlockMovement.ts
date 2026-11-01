import type { Ref } from 'vue'
import type { Editor } from '@tiptap/vue-3'
import { Fragment } from '@tiptap/pm/model'
import { NodeSelection, Selection, TextSelection } from '@tiptap/pm/state'

export function useBlockMovement(editor: Editor, selectionRevision: Ref<number>) {
    function getCurrentBlockIndex() {
        selectionRevision.value
        const { state } = editor
        const selection = state.selection
        let position: number | null = null

        if (selection instanceof Selection && !selection.empty) {
            position = selection.$from.depth > 0 ? selection.$from.before(1) : selection.from
        } else if (selection.$from.depth > 0) {
            position = selection.$from.before(1)
        }

        if (position === null) return -1
        const blocks: number[] = []
        state.doc.forEach((_node, blockPosition) => blocks.push(blockPosition))
        return blocks.indexOf(position)
    }

    function canMoveBlock(direction: -1 | 1) {
        const index = getCurrentBlockIndex()
        const blockTotal = editor.state.doc.childCount
        return direction < 0 ? index > 0 : index >= 0 && index < blockTotal - 1
    }

    function moveBlock(direction: -1 | 1) {
        const { state, view } = editor
        const index = getCurrentBlockIndex()
        const blocks: Array<{ node: typeof state.doc; pos: number }> = []
        state.doc.forEach((node, pos) => blocks.push({ node, pos }))
        const current = blocks[index]
        const neighbor = blocks[index + direction]
        if (!current || !neighbor) return

        const from = direction < 0 ? neighbor.pos : current.pos
        const to = direction < 0 ? current.pos + current.node.nodeSize : neighbor.pos + neighbor.node.nodeSize
        const replacement = direction < 0 ? [current.node, neighbor.node] : [neighbor.node, current.node]
        const newPosition = direction < 0 ? neighbor.pos : current.pos + neighbor.node.nodeSize
        const transaction = state.tr.replaceWith(from, to, Fragment.fromArray(replacement))
        const movedNode = transaction.doc.nodeAt(newPosition)
        if (movedNode?.isAtom && NodeSelection.isSelectable(movedNode)) {
            transaction.setSelection(NodeSelection.create(transaction.doc, newPosition))
        } else {
            transaction.setSelection(TextSelection.near(transaction.doc.resolve(newPosition + 1)))
        }
        view.dispatch(transaction.scrollIntoView())
    }

    return { canMoveBlock, moveBlock }
}
