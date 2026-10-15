import { Extension } from '@tiptap/core'
import { Plugin, PluginKey } from '@tiptap/pm/state'
import { Decoration, DecorationSet, type EditorView } from '@tiptap/pm/view'
import { CellSelection } from '@tiptap/pm/tables'
import { getCellPosition } from '../../shared/nodeViewUtils'

export const TableSelectionHighlight = Extension.create({
    name: 'tableSelectionHighlight',
    addProseMirrorPlugins() {
        let stopCellDrag = () => {}
        return [new Plugin({
            props: {
                handleDOMEvents: {
                    mousedown: (view, event) => {
                        if (event.button !== 0) return false
                        const anchorPosition = getCellPosition(view, event.target)
                        const anchorCell = event.target instanceof Element ? event.target.closest('td, th') : null
                        const anchorTable = anchorCell?.closest('table')
                        if (anchorPosition === null || !anchorTable) return false

                        stopCellDrag()
                        const document = view.dom.ownerDocument
                        const startX = event.clientX
                        const startY = event.clientY
                        const endDrag = () => {
                            document.removeEventListener('mousemove', onMouseMove, true)
                            document.removeEventListener('mouseup', endDrag, true)
                            stopCellDrag = () => {}
                        }
                        const onMouseMove = (moveEvent: MouseEvent) => {
                            if (!(moveEvent.buttons & 1)) {
                                endDrag()
                                return
                            }
                            if (Math.hypot(moveEvent.clientX - startX, moveEvent.clientY - startY) < 4) return

                            const headCell = document.elementFromPoint(moveEvent.clientX, moveEvent.clientY)?.closest('td, th')
                            if (!headCell || headCell.closest('table') !== anchorTable) return
                            const headPosition = getCellPosition(view, headCell)
                            if (headPosition === null) return

                            const selection = CellSelection.create(view.state.doc, anchorPosition, headPosition)
                            if (!selection.eq(view.state.selection)) view.dispatch(view.state.tr.setSelection(selection))
                            moveEvent.preventDefault()
                        }

                        document.addEventListener('mousemove', onMouseMove, true)
                        document.addEventListener('mouseup', endDrag, true)
                        stopCellDrag = endDrag
                        return false
                    },
                },
                decorations: (state) => {
                    const decorations: Decoration[] = []
                    const { selection } = state
                    const rootPosition = selection.$from.depth > 0 ? selection.$from.before(1) : null
                    if (rootPosition !== null) {
                        const rootNode = state.doc.nodeAt(rootPosition)
                        if (rootNode?.isBlock)
                            decorations.push(Decoration.node(rootPosition, rootPosition + rootNode.nodeSize, { class: 'is-active-block' }))
                    }

                    for (let depth = selection.$from.depth; depth > 0; depth--) {
                        const node = selection.$from.node(depth)
                        const position = selection.$from.before(depth)
                        if (node.type.name === 'table')
                            decorations.push(Decoration.node(position, position + node.nodeSize, { class: 'is-active-table' }))
                        if (node.type.name === 'tableCell' || node.type.name === 'tableHeader')
                            decorations.push(Decoration.node(position, position + node.nodeSize, { class: 'is-active-cell' }))
                    }
                    return DecorationSet.create(state.doc, decorations)
                },
            },
            view: () => ({ destroy: () => stopCellDrag() }),
        })]
    },
})
