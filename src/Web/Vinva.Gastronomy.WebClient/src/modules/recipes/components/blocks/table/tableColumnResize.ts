import { Extension } from '@tiptap/core'
import { Plugin, PluginKey } from '@tiptap/pm/state'
import { Decoration, DecorationSet, type EditorView } from '@tiptap/pm/view'
import { TableMap } from '@tiptap/pm/tables'
import { getCellPosition } from '../../shared/nodeViewUtils'

type ColumnResizePluginState = {
    activeCellPos: number
    activeLeftCol: number
    dragging: { startX: number; leftCol: number; rightCol: number; pair: number; leftWidth: number } | null
}

const tableColumnResizeKey = new PluginKey<ColumnResizePluginState>('recipeTableColumnResize')

const COLUMN_MIN_WIDTH = 80
const COLUMN_HIT_AREA = 6

function findResizeBoundary(view: EditorView, event: MouseEvent) {
    if (!view.editable) return null
    const cell = event.target instanceof Element ? event.target.closest('td, th') : null
    if (!cell || !view.dom.contains(cell)) return null
    const rect = cell.getBoundingClientRect()
    if (rect.width <= 0) return null
    const nearLeft = event.clientX - rect.left <= COLUMN_HIT_AREA
    const nearRight = rect.right - event.clientX <= COLUMN_HIT_AREA
    if (!nearLeft && !nearRight) return null

    const cellPos = getCellPosition(view, cell)
    if (cellPos === null) return null
    const $cell = view.state.doc.resolve(cellPos)
    const table = $cell.node(-1)
    const node = $cell.nodeAfter
    if (!table || !node) return null
    const map = TableMap.get(table)
    const start = $cell.start(-1)
    const rel = cellPos - start
    const cellCol = map.colCount(rel)
    const colspan = Number(node.attrs.colspan ?? 1)

    if (nearLeft) {
        if (cellCol === 0) return null
        const slot = map.map.indexOf(rel)
        const row = Math.floor(slot / map.width)
        const leftRel = map.map[row * map.width + (cellCol - 1)]
        const leftNode = leftRel != null ? table.nodeAt(leftRel) : null
        if (leftRel == null || !leftNode) return null
        return { cellPos: start + leftRel, leftCol: map.colCount(leftRel) + Number(leftNode.attrs.colspan ?? 1) - 1 }
    }

    const rightCol = cellCol + colspan - 1
    if (rightCol >= map.width - 1) return null
    return { cellPos, leftCol: rightCol }
}

function setColumnResizeHandle(view: EditorView, cellPos: number, leftCol: number) {
    const current = tableColumnResizeKey.getState(view.state)
    if (current && current.activeCellPos === cellPos && current.activeLeftCol === leftCol) return
    view.dispatch(view.state.tr.setMeta(tableColumnResizeKey, { setCellPos: cellPos, setLeftCol: leftCol }))
    view.dom.classList.toggle('resize-cursor', cellPos > -1)
}

function syncResizeHandle(view: EditorView, event: MouseEvent) {
    if (!view.editable) return
    if (tableColumnResizeKey.getState(view.state)?.dragging) return
    const info = findResizeBoundary(view, event)
    setColumnResizeHandle(view, info ? info.cellPos : -1, info ? info.leftCol : -1)
}

function setCellColumnWidths(view: EditorView, cellPos: number, leftCol: number, leftWidth: number, rightWidth: number) {
    const $cell = view.state.doc.resolve(cellPos)
    const table = $cell.node(-1)
    if (!table) return
    const map = TableMap.get(table)
    const start = $cell.start(-1)
    const tr = view.state.tr
    let changed = false
    const apply = (col: number, width: number) => {
        if (col < 0 || col >= map.width) return
        for (let row = 0; row < map.height; row++) {
            const pos = map.map[row * map.width + col]
            if (pos == null) continue
            if (row > 0 && map.map[row * map.width + col] === map.map[(row - 1) * map.width + col]) continue
            const cell = table.nodeAt(pos)
            if (!cell) continue
            const attrs = { ...cell.attrs }
            const cellCol = map.colCount(pos)
            const index = attrs.colspan === 1 ? 0 : col - cellCol
            const colwidth = attrs.colwidth ? [...attrs.colwidth] : Array(attrs.colspan).fill(0)
            if (colwidth[index] === width) continue
            colwidth[index] = Math.max(1, Math.round(width))
            attrs.colwidth = colwidth
            tr.setNodeMarkup(start + pos, undefined, attrs)
            changed = true
        }
    }
    apply(leftCol, leftWidth)
    apply(leftCol + 1, rightWidth)
    if (changed) view.dispatch(tr)
}

export const RecipeTableColumnResize = Extension.create({
    name: 'recipeTableColumnResize',
    addProseMirrorPlugins() {
        let detachDrag: (() => void) | null = null
        return [new Plugin<ColumnResizePluginState>({
            key: tableColumnResizeKey,
            state: {
                init: () => ({ activeCellPos: -1, activeLeftCol: -1, dragging: null }),
                apply(tr, prev) {
                    const meta = tr.getMeta(tableColumnResizeKey)
                    if (meta) {
                        if (meta.dragging !== undefined) return { ...prev, dragging: meta.dragging }
                        if (meta.setCellPos !== undefined) return { ...prev, activeCellPos: meta.setCellPos, activeLeftCol: meta.setLeftCol }
                    }
                    return {
                        activeCellPos: prev.activeCellPos > -1 ? tr.mapping.map(prev.activeCellPos, -1) : -1,
                        activeLeftCol: prev.activeLeftCol,
                        dragging: prev.dragging,
                    }
                },
            },
            props: {
                handleDOMEvents: {
                    mousemove: (view, event) => {
                        syncResizeHandle(view, event as MouseEvent)
                        return false
                    },
                    mouseleave: (view) => {
                        const ps = tableColumnResizeKey.getState(view.state)
                        if (view.editable && ps && ps.activeCellPos > -1)
                            setColumnResizeHandle(view, -1, -1)
                        return false
                    },
                    mousedown: (view, event) => {
                        if (event.button !== 0) return false
                        const info = findResizeBoundary(view, event as MouseEvent)
                        if (!info) return false

                        detachDrag?.()
                        const tableEl = event.target instanceof Element ? event.target.closest('table') : null
                        const cols = tableEl ? [...tableEl.querySelectorAll('col')] : []
                        const leftEl = cols[info.leftCol]
                        const rightEl = cols[info.leftCol + 1]
                        if (!leftEl || !rightEl) return true
                        const leftWidth = leftEl.getBoundingClientRect().width
                        const rightWidth = rightEl.getBoundingClientRect().width
                        const pair = leftWidth + rightWidth
                        if (!(pair > 0)) return true

                        view.dispatch(view.state.tr.setMeta(tableColumnResizeKey, {
                            dragging: { startX: event.clientX, leftCol: info.leftCol, rightCol: info.leftCol + 1, pair, leftWidth },
                        }))

                        event.preventDefault()
                        const document = view.dom.ownerDocument
                        const onMove = (moveEvent: MouseEvent) => {
                            if (!(moveEvent.buttons & 1)) {
                                onUp()
                                return
                            }
                            const state = tableColumnResizeKey.getState(view.state)
                            if (!state?.dragging) return
                            const max = Math.max(COLUMN_MIN_WIDTH, state.dragging.pair - COLUMN_MIN_WIDTH)
                            const width = Math.min(max, Math.max(COLUMN_MIN_WIDTH, state.dragging.leftWidth + moveEvent.clientX - state.dragging.startX))
                            leftEl.style.width = `${width}px`
                            rightEl.style.width = `${state.dragging.pair - width}px`
                        }
                        const onUp = () => {
                            document.removeEventListener('mousemove', onMove, true)
                            document.removeEventListener('mouseup', onUp, true)
                            detachDrag = null
                            const state = tableColumnResizeKey.getState(view.state)
                            if (state?.dragging) {
                                const width = Math.min(Math.max(COLUMN_MIN_WIDTH, state.dragging.pair - COLUMN_MIN_WIDTH),
                                    Math.max(COLUMN_MIN_WIDTH, leftEl.getBoundingClientRect().width))
                                setCellColumnWidths(view, info.cellPos, state.dragging.leftCol, width, state.dragging.pair - width)
                            }
                            view.dispatch(view.state.tr.setMeta(tableColumnResizeKey, { dragging: null }))
                        }
                        detachDrag = onUp
                        document.addEventListener('mousemove', onMove, true)
                        document.addEventListener('mouseup', onUp, true)
                        return true
                    },
                },
                decorations: (state) => {
                    const ps = tableColumnResizeKey.getState(state)
                    if (!ps || ps.activeCellPos <= -1) return DecorationSet.empty
                    const $cell = state.doc.resolve(ps.activeCellPos)
                    const table = $cell.node(-1)
                    const node = $cell.nodeAfter
                    if (!table || !node) return DecorationSet.empty
                    const map = TableMap.get(table)
                    const start = $cell.start(-1)
                    const rel = ps.activeCellPos - start
                    const leftCol = map.colCount(rel) + Number(node.attrs.colspan ?? 1) - 1
                    const end = ps.activeCellPos + node.nodeSize - 1
                    const decorations: Decoration[] = []
                    decorations.push(Decoration.widget(end, () => {
                        const el = document.createElement('div')
                        el.className = 'column-resize-handle'
                        return el
                    }))
                    const markCols = ps.dragging ? [leftCol, leftCol + 1] : [leftCol]
                    for (const col of markCols) {
                        if (col < 0 || col >= map.width) continue
                        for (let row = 0; row < map.height; row++) {
                            const index = row * map.width + col
                            if (row > 0 && map.map[index] === map.map[index - map.width]) continue
                            const pos = map.map[index]
                            if (pos == null) continue
                            const cell = table.nodeAt(pos)
                            if (!cell) continue
                            decorations.push(Decoration.node(start + pos, start + pos + cell.nodeSize,
                                { class: ps.dragging ? 'column-resize-dragging' : 'column-resize-hover' }))
                        }
                    }
                    return DecorationSet.create(state.doc, decorations)
                },
            },
            view: () => ({ destroy: () => detachDrag?.() }),
        })]
    },
})
