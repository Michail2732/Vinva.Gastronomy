import { Extension } from '@tiptap/core'
import type { Editor } from '@tiptap/core'
import { Fragment } from '@tiptap/pm/model'
import { NodeSelection, Plugin, PluginKey } from '@tiptap/pm/state'
import { Decoration, DecorationSet, type EditorView } from '@tiptap/pm/view'
import { findBlockPosByElement, getActiveBlockPos } from '../../shared/nodeViewUtils'

type BlockDragState = {
    nodePos: number
    nodeSize: number
    originPos: number
}

type BlockDragPluginState = {
    dragging: BlockDragState | null
}

const FLIP_DURATION = 180
const DIRECTION_EPSILON = 1

const blockDragKey = new PluginKey<BlockDragPluginState>('recipeBlockDrag')

export const RecipeBlockDrag = Extension.create({
    name: 'recipeBlockDrag',
    addProseMirrorPlugins() {
        return [new Plugin<BlockDragPluginState>({
            key: blockDragKey,
            state: {
                init: () => ({ dragging: null }),
                apply(tr, prev) {
                    const meta = tr.getMeta(blockDragKey) as { dragging?: BlockDragState | null } | undefined
                    if (meta && meta.dragging !== undefined) return { dragging: meta.dragging }
                    return prev
                },
            },
            props: {
                handleDOMEvents: {
                    pointerdown: (view, event) => {
                        if (event.button !== 0) return false
                        selectBlockFromEvent(view, event)
                        return false
                    },
                    focusin: (view, event) => {
                        selectBlockFromEvent(view, event)
                        return false
                    },
                },
                decorations: (state) => {
                    const decorations: Decoration[] = []
                    const dragging = blockDragKey.getState(state)?.dragging ?? null

                    if (dragging) {
                        decorations.push(Decoration.node(dragging.nodePos, dragging.nodePos + dragging.nodeSize, { class: 'block-dragging-node' }))
                        return DecorationSet.create(state.doc, decorations)
                    }

                    const activePos = getActiveBlockPos(state.doc, state.selection)
                    const activeNode = activePos !== null ? state.doc.nodeAt(activePos) : null
                    if (activePos !== null && activePos >= 0 && activeNode) {
                        decorations.push(Decoration.node(activePos, activePos + activeNode.nodeSize, { class: 'block-drag-target' }))
                    }
                    return DecorationSet.create(state.doc, decorations)
                },
            },
        })]
    },
})

export type BlockDragController = {
    start: (event: PointerEvent, blockEl: HTMLElement) => void
}

const controllers = new WeakMap<Editor, BlockDragController>()

export function getBlockDragController(editor: Editor): BlockDragController {
    let controller = controllers.get(editor)
    if (!controller) {
        controller = createBlockDragController(editor)
        controllers.set(editor, controller)
    }
    return controller
}

function createBlockDragController(editor: Editor): BlockDragController {
    function start(event: PointerEvent, blockEl: HTMLElement) {
        if (event.button !== 0 || !editor.isEditable) return
        if (blockDragKey.getState(editor.state)?.dragging) return

        const view = editor.view
        const nodePos = findBlockPosByElement(view, blockEl)
        if (nodePos === null) return
        const node = view.state.doc.nodeAt(nodePos)
        if (!node) return

        event.preventDefault()
        view.dom.classList.add('block-dragging')
        view.dispatch(view.state.tr
            .setSelection(NodeSelection.create(view.state.doc, nodePos))
            .setMeta(blockDragKey, { dragging: { nodePos, nodeSize: node.nodeSize, originPos: nodePos } }))

        const doc = view.dom.ownerDocument
        let lastY = event.clientY

        const onPointerMove = (moveEvent: PointerEvent) => {
            const current = blockDragKey.getState(view.state)?.dragging
            if (!current) return
            const dy = moveEvent.clientY - lastY
            if (Math.abs(dy) < DIRECTION_EPSILON) return
            lastY = moveEvent.clientY
            const direction = dy > 0 ? 1 : -1
            const dropPos = computeDropPos(view, moveEvent.clientY, current, direction)
            if (dropPos === current.nodePos) return
            applyMove(view, current, dropPos)
        }
        const onPointerUp = () => {
            cleanup()
            view.dispatch(view.state.tr.setMeta(blockDragKey, { dragging: null }))
        }
        const onKeyDown = (keyEvent: KeyboardEvent) => {
            if (keyEvent.key !== 'Escape') return
            const current = blockDragKey.getState(view.state)?.dragging
            cleanup()
            if (current && current.nodePos !== current.originPos) {
                applyMove(view, current, current.originPos)
            }
            view.dispatch(view.state.tr.setMeta(blockDragKey, { dragging: null }))
        }
        const cleanup = () => {
            doc.removeEventListener('pointermove', onPointerMove)
            doc.removeEventListener('pointerup', onPointerUp)
            doc.removeEventListener('keydown', onKeyDown)
            view.dom.classList.remove('block-dragging')
        }

        doc.addEventListener('pointermove', onPointerMove)
        doc.addEventListener('pointerup', onPointerUp)
        doc.addEventListener('keydown', onKeyDown)
    }

    return { start }
}

function layoutTop(el: HTMLElement): number {
    const top = el.getBoundingClientRect().top
    const transform = getComputedStyle(el).transform
    if (!transform || transform === 'none') return top
    return top - new DOMMatrixReadOnly(transform).m42
}

function computeDropPos(view: EditorView, clientY: number, state: BlockDragState, direction: number): number {
    const { nodePos: from, nodeSize } = state
    const doc = view.state.doc
    const draggingDown = direction > 0

    let pos = 0
    for (let i = 0; i < doc.childCount; i++) {
        const node = doc.child(i)
        if (pos === from) {
            pos += nodeSize
            continue
        }
        const dom = view.nodeDOM(pos)
        if (dom instanceof HTMLElement) {
            const top = layoutTop(dom)
            const bottom = top + dom.getBoundingClientRect().height
            if (draggingDown ? top > clientY : bottom > clientY) return pos
        }
        pos += node.nodeSize
    }
    return doc.content.size
}

function applyMove(view: EditorView, state: BlockDragState, dropPos: number) {
    const { nodePos: from, nodeSize, originPos } = state
    const node = view.state.doc.nodeAt(from)
    if (!node) return

    let insertPos = dropPos
    if (dropPos > from) insertPos -= nodeSize
    if (insertPos < 0 || insertPos > view.state.doc.content.size || insertPos === from) return

    const before = captureTops(view)
    const tr = view.state.tr.delete(from, from + nodeSize)
    tr.insert(insertPos, Fragment.from(node))
    tr.setSelection(NodeSelection.create(tr.doc, insertPos))
    tr.setMeta(blockDragKey, { dragging: { nodePos: insertPos, nodeSize, originPos } })
    view.dispatch(tr)
    playFlip(before, captureTops(view))
}

function captureTops(view: EditorView): Map<Element, number> {
    const tops = new Map<Element, number>()
    view.state.doc.forEach((_node, pos) => {
        const dom = view.nodeDOM(pos)
        if (dom instanceof HTMLElement) tops.set(dom, layoutTop(dom))
    })
    return tops
}

function playFlip(before: Map<Element, number>, after: Map<Element, number>) {
    after.forEach((top, element) => {
        const previous = before.get(element)
        if (previous === undefined) return
        const delta = previous - top
        if (Math.abs(delta) < 1) return
        runningAnimations.get(element)?.cancel()
        const animation = element.animate(
            [{ transform: `translateY(${delta}px)` }, { transform: 'translateY(0)' }],
            { duration: FLIP_DURATION, easing: 'ease-out' },
        )
        runningAnimations.set(element, animation)
        animation.addEventListener('finish', () => runningAnimations.delete(element))
    })
}

const runningAnimations = new WeakMap<Element, Animation>()

function selectBlockFromEvent(view: EditorView, event: Event) {
    const target = event.target
    if (!(target instanceof HTMLElement)) return
    if (target.closest('.block-drag-handle')) return
    if (target.isContentEditable) return

    const blockEl = target.closest<HTMLElement>('.recipe-block')
    if (!blockEl || !view.dom.contains(blockEl)) return

    const position = findBlockPosByElement(view, blockEl)
    if (position === null) return

    const selection = NodeSelection.create(view.state.doc, position)
    if (selection.eq(view.state.selection)) return
    view.dispatch(view.state.tr.setSelection(selection))
}
