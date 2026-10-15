import type { Editor } from '@tiptap/core'
import { CellSelection, TableMap } from '@tiptap/pm/tables'
import { NodeSelection, TextSelection } from '@tiptap/pm/state'

export function selectCellScope(editor: Editor) {
    const { state, view } = editor
    const { selection, doc } = state
    const $from = selection.$from
    let cellDepth = -1
    let tableDepth = -1
    let paragraphDepth = -1
    for (let depth = 1; depth <= $from.depth; depth++) {
        const name = $from.node(depth).type.name
        if (name === 'tableCell' || name === 'tableHeader') cellDepth = depth
        if (name === 'table') tableDepth = depth
        if (name === 'groupBox') paragraphDepth = depth
    }
    if (paragraphDepth > -1) {
        const paragraphNode = $from.node(paragraphDepth)
        const paragraphPos = $from.before(paragraphDepth)
        const contentFrom = paragraphPos + 1
        const contentTo = paragraphPos + paragraphNode.nodeSize - 1
        if (selection.from <= contentFrom && selection.to >= contentTo) return false
        view.dispatch(state.tr.setSelection(TextSelection.create(doc, contentFrom, contentTo)))
        return true
    }
    if (cellDepth < 0 || tableDepth < 0) return false

    const cell = $from.node(cellDepth)
    const cellPos = $from.before(cellDepth)
    const fullFrom = cellPos + 2
    const fullTo = cellPos + cell.nodeSize - 2

    const tableStart = $from.before(tableDepth) + 1
    const map = TableMap.get($from.node(tableDepth))
    const cells = map.cellsInRect({ left: 0, top: 0, right: map.width, bottom: map.height })
    const firstOffset = cells[0]
    const lastOffset = cells[cells.length - 1]
    if (firstOffset === undefined || lastOffset === undefined) return false
    const firstCell = tableStart + firstOffset
    const lastCell = tableStart + lastOffset
    const fullTable = new CellSelection(doc.resolve(firstCell), doc.resolve(lastCell))

    if (selection instanceof CellSelection) {
        const coversTable = (selection.$anchorCell.pos === firstCell && selection.$headCell.pos === lastCell)
            || (selection.$anchorCell.pos === lastCell && selection.$headCell.pos === firstCell)
        if (coversTable) return false
        view.dispatch(state.tr.setSelection(fullTable))
        return true
    }

    if (selection.from <= fullFrom && selection.to >= fullTo) {
        view.dispatch(state.tr.setSelection(fullTable))
        return true
    }

    view.dispatch(state.tr.setSelection(TextSelection.create(doc, fullFrom, fullTo)))
    return true
}
