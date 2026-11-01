import { ref } from 'vue'
import type { Ref } from 'vue'
import type { Editor } from '@tiptap/vue-3'
import { NodeSelection, TextSelection } from '@tiptap/pm/state'
import type { IngredientRowViewModel, IngredientTextStyle } from '../ingredientListModel'

type TextAlignment = 'left' | 'center' | 'right'
type TextMark = 'bold' | 'italic' | 'underline' | 'strike'
type HeadingLevel = 0 | 1 | 2 | 3

export const fontFamilies = [
    { label: 'По умолчанию', value: '' },
    { label: 'Lora', value: 'Lora' },
    { label: 'Source Sans 3', value: 'Source Sans 3' },
    { label: 'Open Sans', value: 'Open Sans' },
    { label: 'Roboto Slab', value: 'Roboto Slab' },
    { label: 'Playfair Display', value: 'Playfair Display' },
]

export function useTextFormatting(editor: Editor, savedTextRange: Ref<{ from: number; to: number } | null>) {
    const fontSize = ref(16)
    const fontColor = ref('#202522')
    const fontBackgroundColor = ref('')
    const fontFamily = ref('')
    const activeTitleBlockPosition = ref<number | null>(null)
    const activeIngredientId = ref<string | null>(null)

    function findActiveIngredient(): { position: number; attrs: Record<string, unknown>; ingredient: IngredientRowViewModel } | null {
        const id = activeIngredientId.value
        if (!id) return null
        const matches: { position: number; attrs: Record<string, unknown>; ingredient: IngredientRowViewModel }[] = []
        editor.state.doc.descendants((node, position) => {
            if (node.type.name !== 'ingredientsSet') return true
            const ingredients = node.attrs.ingredients as IngredientRowViewModel[] | undefined
            const ingredient = ingredients?.find((row) => row.id === id)
            if (!ingredient) return false
            matches.push({ position, attrs: node.attrs, ingredient })
            return false
        })
        return matches[0] ?? null
    }

    function updateActiveIngredientStyle(field: keyof IngredientTextStyle, value?: string | number) {
        const active = findActiveIngredient()
        if (!active) return false
        const currentStyle = active.ingredient.textStyle ?? {}
        const nextStyle = { ...currentStyle } as IngredientTextStyle
        if (['bold', 'italic', 'underline', 'strike'].includes(field)) {
            const mark = field as TextMark
            nextStyle[mark] = !currentStyle[mark]
        } else if (value === undefined || value === '') {
            delete nextStyle[field]
        } else {
            Object.assign(nextStyle, { [field]: value })
        }
        const ingredients = (active.attrs.ingredients as IngredientRowViewModel[]).map((row) => row.id === active.ingredient.id
            ? { ...row, textStyle: nextStyle }
            : row)
        editor.view.dispatch(editor.state.tr.setNodeMarkup(active.position, undefined, { ...active.attrs, ingredients }))
        return true
    }

    function onEditorFocusIn(event: FocusEvent) {
        const target = event.target
        activeIngredientId.value = target instanceof HTMLInputElement && target.classList.contains('ingredient-row__name')
            ? target.dataset.ingredientId ?? null
            : null
        const style = findActiveIngredient()?.ingredient.textStyle
        if (!activeIngredientId.value) return
        fontSize.value = style?.fontSize ?? 16
        fontFamily.value = style?.fontFamily ?? ''
        fontColor.value = style?.color ?? '#202522'
    }

    function rememberBlockTitleTarget(event: PointerEvent) {
        const target = event.target
        const label = target instanceof Element ? target.closest('.group-box__heading, .recipe-block__label') : null
        const block = label?.closest('.group-box-block, .recipe-block')
        if (!block) {
            activeTitleBlockPosition.value = null
            return
        }

        let blockPosition: number | null = null
        editor.state.doc.descendants((node, position) => {
            if (['groupBox', 'ingredientsSet'].includes(node.type.name)
                && editor.view.nodeDOM(position) === block) {
                blockPosition = position
                return false
            }
            return true
        })
        activeTitleBlockPosition.value = blockPosition
    }

    function restoreTextRange() {
        const range = savedTextRange.value
        if (!range || range.from === range.to || range.to > editor.state.doc.content.size) return false
        return editor.chain().setTextSelection(range).run()
    }

    function toggleMark(mark: TextMark) {
        if (updateActiveIngredientStyle(mark)) return
        const commands = editor.chain().focus()
        if (mark === 'bold') commands.toggleBold().run()
        else if (mark === 'italic') commands.toggleItalic().run()
        else if (mark === 'underline') commands.toggleUnderline().run()
        else commands.toggleStrike().run()
    }

    function isTextMarkActive(mark: TextMark) {
        return findActiveIngredient()?.ingredient.textStyle?.[mark] ?? editor.isActive(mark)
    }

    function isTextAligned(alignment: TextAlignment) {
        const activeIngredient = findActiveIngredient()
        if (activeIngredient) return activeIngredient.ingredient.textStyle?.textAlign === alignment
        return ['paragraph', 'heading', 'groupBox', 'textBlock']
            .some((type) => editor.isActive(type, { textAlign: alignment }))
    }

    function setTextAlignment(alignment: TextAlignment) {
        if (findActiveIngredient()) {
            updateActiveIngredientStyle('textAlign', alignment)
            return
        }
        editor.chain().focus().setTextAlign(alignment).run()
    }

    function applyFontSize(value: number | null) {
        if (!value) return
        if (updateActiveIngredientStyle('fontSize', value)) return
        if (!restoreTextRange()) return
        editor.chain().setMark('textStyle', { fontSize: `${value}px` }).run()
    }

    function applyFontColor() {
        if (updateActiveIngredientStyle('color', fontColor.value)) return
        const position = activeTitleBlockPosition.value
        const node = position === null ? null : editor.state.doc.nodeAt(position)
        if (position !== null && node && ['groupBox', 'ingredientsSet'].includes(node.type.name)) {
            editor.view.dispatch(editor.state.tr.setNodeMarkup(position, undefined, {
                ...node.attrs,
                titleColor: fontColor.value,
            }))
            return
        }
        if (!restoreTextRange()) return
        editor.chain().setColor(fontColor.value).run()
    }

    function applyFontFamily() {
        if (updateActiveIngredientStyle('fontFamily', fontFamily.value || undefined)) return
        if (!restoreTextRange()) return
        editor.chain().setMark('textStyle', { fontFamily: fontFamily.value || null }).run()
    }

    function applyFontBackgroundColor(value: string) {
        if (findActiveIngredient()) {
            updateActiveIngredientStyle('backgroundColor', value || undefined)
            return
        }
        const chain = editor.chain().focus()
        if (value) chain.setMark('textStyle', { backgroundColor: value }).run()
        else chain.unsetMark('textStyle').run()
    }

    function applyHeadingLevel(level: HeadingLevel) {
        if (level === 0) {
            fontSize.value = 16
            editor.chain().focus().setParagraph().setMark('textStyle', { fontSize: '16px' }).run()
            return
        }
        const fontSizeByLevel = { 1: 28, 2: 22, 3: 18 }
        fontSize.value = fontSizeByLevel[level]
        editor.chain().focus().setHeading({ level }).setMark('textStyle', { fontSize: `${fontSize.value}px` }).run()
    }

    function onFontSizeUpdate(value: number | null) {
        if (value !== null) fontSize.value = value
        applyFontSize(value)
    }

    function onFontColorUpdate(value: string) {
        fontColor.value = value
        applyFontColor()
    }

    function onFontFamilyUpdate(value: string) {
        fontFamily.value = value
        applyFontFamily()
    }

    function onFontBackgroundColorUpdate(value: string) {
        fontBackgroundColor.value = value
        applyFontBackgroundColor(value)
    }

    function changeFontSize(delta: -1 | 1) {
        fontSize.value = Math.max(8, Math.min(72, fontSize.value + delta))
        applyFontSize(fontSize.value)
    }

    return {
        fontSize,
        fontColor,
        fontBackgroundColor,
        fontFamily,
        fontFamilies,
        toggleMark,
        isTextMarkActive,
        isTextAligned,
        setTextAlignment,
        onFontSizeUpdate,
        onFontColorUpdate,
        onFontFamilyUpdate,
        onFontBackgroundColorUpdate,
        changeFontSize,
        applyHeadingLevel,
        onEditorFocusIn,
        rememberBlockTitleTarget,
    }
}
