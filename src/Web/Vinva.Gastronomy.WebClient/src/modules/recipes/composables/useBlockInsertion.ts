import { ref } from 'vue'
import type { Editor } from '@tiptap/vue-3'
import { NodeSelection, TextSelection } from '@tiptap/pm/state'
import { createRecipeBlockContent, type RecipeBlockType } from '../recipeBlockCatalog'

export function useBlockInsertion(editor: Editor) {
    const dragType = ref<RecipeBlockType | null>(null)
    const dropPosition = ref<number | null>(null)
    const pointerOrigin = ref<{ x: number; y: number; type: RecipeBlockType } | null>(null)
    const pointerDragActive = ref(false)
    const suppressPaletteClick = ref(false)

    function insertBlock(type: RecipeBlockType) {
        const { selection } = editor.state
        const $to = selection.$to
        const node = editor.schema.nodeFromJSON(createRecipeBlockContent(type))

        let groupDepth = -1
        for (let depth = $to.depth; depth > 0; depth--) {
            if ($to.node(depth).type.name === 'groupBox') {
                groupDepth = depth
                break
            }
        }

        let replaceFrom = -1
        let replaceTo = -1
        let position = selection.to
        if (groupDepth > -1) {
            const childDepth = groupDepth + 1
            const child = $to.depth >= childDepth ? $to.node(childDepth) : null
            if (child && (child.type.name === 'textBlock' || child.type.name === 'paragraph') && child.content.size === 0) {
                replaceFrom = $to.before(childDepth)
                replaceTo = replaceFrom + child.nodeSize
            } else {
                position = $to.end(groupDepth)
            }
        } else if ($to.depth > 0) {
            position = $to.after(1)
        }

        const transaction = replaceFrom > -1
            ? editor.state.tr.replaceWith(replaceFrom, replaceTo, node)
            : editor.state.tr.insert(position, node)
        const anchor = replaceFrom > -1 ? replaceFrom : position
        if (node.isAtom && NodeSelection.isSelectable(node)) {
            transaction.setSelection(NodeSelection.create(transaction.doc, anchor))
        } else {
            transaction.setSelection(TextSelection.near(transaction.doc.resolve(anchor + 1)))
        }
        editor.view.dispatch(transaction.scrollIntoView())
    }

    function onPaletteClick(type: RecipeBlockType) {
        if (suppressPaletteClick.value) {
            suppressPaletteClick.value = false
            return
        }
        insertBlock(type)
    }

    function onPalettePointerDown(event: PointerEvent) {
        if (event.button !== 0) return
        const target = event.target
        const blockType = target instanceof HTMLElement ? target.closest<HTMLElement>('[data-block-type]')?.dataset.blockType as RecipeBlockType | undefined : undefined
        if (!blockType) return
        pointerOrigin.value = { x: event.clientX, y: event.clientY, type: blockType }
    }

    function onWindowPointerMove(event: PointerEvent) {
        const origin = pointerOrigin.value
        if (!origin) return
        if (!pointerDragActive.value && Math.hypot(event.clientX - origin.x, event.clientY - origin.y) < 8) return
        pointerDragActive.value = true
        dragType.value = origin.type
        dropPosition.value = editor.view.posAtCoords({ left: event.clientX, top: event.clientY })?.pos ?? editor.state.doc.content.size
    }

    function onWindowPointerUp(event: PointerEvent) {
        const origin = pointerOrigin.value
        if (!origin) return
        if (pointerDragActive.value) {
            const dropTarget = document.elementFromPoint(event.clientX, event.clientY)
            if (dropTarget instanceof HTMLElement && dropTarget.closest('.paper-shell')) {
                const position = editor.view.posAtCoords({ left: event.clientX, top: event.clientY })?.pos ?? dropPosition.value ?? editor.state.doc.content.size
                const node = editor.schema.nodeFromJSON(createRecipeBlockContent(origin.type))
                editor.view.dispatch(editor.state.tr.insert(position, node).scrollIntoView())
                suppressPaletteClick.value = true
            }
        }
        pointerOrigin.value = null
        pointerDragActive.value = false
        finishPaletteDrag()
    }

    function finishPaletteDrag() {
        dragType.value = null
        dropPosition.value = null
    }

    function onCanvasDragOver(event: DragEvent) {
        if (!dragType.value) return
        dropPosition.value = editor.view.posAtCoords({ left: event.clientX, top: event.clientY })?.pos ?? editor.state.doc.content.size
    }

    function onCanvasDragLeave(event: DragEvent) {
        const currentTarget = event.currentTarget
        if (currentTarget instanceof HTMLElement && event.relatedTarget instanceof Node && currentTarget.contains(event.relatedTarget)) return
        dropPosition.value = null
    }

    function onCanvasDrop(event: DragEvent) {
        const type = dragType.value
        if (!type) return
        const position = editor.view.posAtCoords({ left: event.clientX, top: event.clientY })?.pos ?? dropPosition.value ?? editor.state.doc.content.size
        const node = editor.schema.nodeFromJSON(createRecipeBlockContent(type))
        editor.view.dispatch(editor.state.tr.insert(position, node).scrollIntoView())
        finishPaletteDrag()
    }

    return {
        dragType,
        dropPosition,
        insertBlock,
        onPaletteClick,
        onPalettePointerDown,
        onWindowPointerMove,
        onWindowPointerUp,
        onCanvasDragOver,
        onCanvasDragLeave,
        onCanvasDrop,
    }
}
