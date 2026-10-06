<template>
    <section class="recipe-workspace">
        <RecipeEditorHeader
            v-model:title="title"
            v-model:preview="preview"
            :save-status="saveStatus"
            :save-status-icon="saveStatusIcon"
            :is-saving="isSaving"
            @save="saveDraft"
        />

        <div class="editor-layout" :class="{ 'is-sidebar-collapsed': sidebarCollapsed }">
            <RecipeBlockPalette
                :collapsed="sidebarCollapsed"
                :block-count="blockCount"
                :categories="blockCategories"
                :active-category="activeCategory"
                :search="blockSearch"
                :groups="visibleBlockGroups"
                @update:collapsed="sidebarCollapsed = $event"
                @update:active-category="activeCategory = $event"
                @update:search="blockSearch = $event"
                @add-block="onPaletteClick"
                @pointerdown="onPalettePointerDown"
            />

            <main class="document-column">
                <RecipeTextToolbar
                    :editor="editor"
                    :font-size="fontSize"
                    :font-color="fontColor"
                    :font-family="fontFamily"
                    :font-families="fontFamilies"
                    :can-move-block="canMoveBlock"
                    :is-text-aligned="isTextAligned"
                    :is-text-mark-active="isTextMarkActive"
                    @update:font-size="onFontSizeUpdate"
                    @update:font-color="onFontColorUpdate"
                    @update:font-family="onFontFamilyUpdate"
                    @toggle-mark="toggleMark"
                    @align="setTextAlignment"
                    @font-step="changeFontSize"
                    @move-block="moveBlock"
                    @heading-level="applyHeadingLevel"
                />
                <RecipeTableToolbar :editor="editor" />
                <RecipeEditorCanvas
                    :editor="editor"
                    :preview="preview"
                    :is-drop-target="Boolean(dragType && !preview)"
                    @pointerdown="rememberBlockTitleTarget"
                    @dragover="onCanvasDragOver"
                    @drop="onCanvasDrop"
                    @dragleave="onCanvasDragLeave"
                />
            </main>
        </div>
    </section>
</template>

<script setup lang="ts">
import { computed, onBeforeUnmount, onMounted, ref, watch } from 'vue'
import { Editor } from '@tiptap/vue-3'
import StarterKit from '@tiptap/starter-kit'
import Placeholder from '@tiptap/extension-placeholder'
import Image from '@tiptap/extension-image'
import Underline from '@tiptap/extension-underline'
import Color from '@tiptap/extension-color'
import TextAlign from '@tiptap/extension-text-align'
import type { JSONContent } from '@tiptap/core'
import { Fragment } from '@tiptap/pm/model'
import { NodeSelection, Selection, TextSelection } from '@tiptap/pm/state'
import type { RecipeDraftViewModel, RecipeIngredientViewModel } from '../types/recipeTypes'
import type { IngredientRowViewModel, IngredientTextStyle } from '../ingredientListModel'
import { useRecipesStore } from '../stores/recipesStore'
import {
    RecipeMedia, RecipeToggle, IngredientList, RecipeParagraph,
    RecipeDivider, ImageGallery, RecipeImage, RecipeTaskList, RecipeTaskItem,
    RecipeTable, RecipeTableRow, RecipeTableHeader, RecipeTableCell, RecipeTextStyle, TableSelectionHighlight,
} from '../recipeBlockExtensions'
import { createRecipeBlockContent, recipeBlockGroups, type RecipeBlockType } from '../recipeBlockCatalog'
import { normalizeRecipeDocument, sampleRecipeDocument } from '../recipeDocumentModel'
import RecipeBlockPalette from './editor/recipeBlockPalette.vue'
import RecipeTextToolbar from './editor/recipeTextToolbar.vue'
import RecipeEditorCanvas from './editor/recipeEditorCanvas.vue'
import RecipeTableToolbar from './editor/recipeTableToolbar.vue'
import RecipeEditorHeader from './editor/recipeEditorHeader.vue'

const props = withDefaults(defineProps<{
    initialTitle?: string
    initialDocument?: JSONContent
}>(), {
    initialTitle: 'Тыквенная паста с шалфеем',
})

const title = ref(props.initialTitle)
const recipesStore = useRecipesStore()
const preview = ref(false)
const sidebarCollapsed = ref(false)
const blockSearch = ref('')
const activeCategory = ref('Все')
const dragType = ref<RecipeBlockType | null>(null)
const dropPosition = ref<number | null>(null)
const pointerOrigin = ref<{ x: number; y: number; type: RecipeBlockType } | null>(null)
const pointerDragActive = ref(false)
const suppressPaletteClick = ref(false)
const fontSize = ref(16)
const fontColor = ref('#202522')
const fontFamily = ref('')
const fontFamilies = [
    { label: 'По умолчанию', value: '' },
    { label: 'Lora', value: 'Lora' },
    { label: 'Source Sans 3', value: 'Source Sans 3' },
    { label: 'Open Sans', value: 'Open Sans' },
    { label: 'Roboto Slab', value: 'Roboto Slab' },
    { label: 'Playfair Display', value: 'Playfair Display' },
]
const savedTextRange = ref<{ from: number; to: number } | null>(null)
const activeTitleBlockPosition = ref<number | null>(null)
const activeIngredientId = ref<string | null>(null)
const selectionRevision = ref(0)
const isSaving = ref(false)
const saveStatus = ref('Черновик не сохранён')
const saveStatusIcon = computed(() => saveStatus.value.startsWith('Не удалось') ? 'pi pi-exclamation-circle' : 'pi pi-check-circle')

type RecipeDocumentAggregate = RecipeDraftViewModel & {
    recipeId: string | null
}

const recipeAggregate = ref<RecipeDocumentAggregate>({
    recipeId: null,
    name: title.value,
    description: '',
    document: normalizeRecipeDocument(props.initialDocument ?? sampleRecipeDocument),
    characteristics: [],
    ingredients: [],
})

const blockCategories = ['Все', ...recipeBlockGroups.map((group) => group.label)]
const visibleBlockGroups = computed(() => {
    const search = blockSearch.value.trim().toLocaleLowerCase('ru')
    return recipeBlockGroups
        .filter((group) => activeCategory.value === 'Все' || group.label === activeCategory.value)
        .map((group) => ({
            ...group,
            items: group.items.filter((item) => !search || `${item.label} ${item.description}`.toLocaleLowerCase('ru').includes(search)),
        }))
        .filter((group) => group.items.length)
})
const blockCount = ref(0)

const editor = new Editor({
    extensions: [
        StarterKit.configure({ horizontalRule: false, blockquote: false, underline: false, trailingNode: false }),
        Placeholder.configure({ placeholder: 'Начните писать или добавьте блок из панели слева...' }),
        Image,
        Underline,
        RecipeTextStyle,
        Color,
        TextAlign.configure({
            types: ['paragraph', 'heading', 'recipeParagraph'],
            alignments: ['left', 'center', 'right'],
        }),
        RecipeTaskList,
        RecipeTaskItem,
        RecipeTable,
        TableSelectionHighlight,
        RecipeTableRow,
        RecipeTableHeader,
        RecipeTableCell,
        RecipeToggle,
        RecipeMedia,
        IngredientList,
        RecipeParagraph,
        RecipeDivider,
        RecipeImage,
        ImageGallery,
    ],
    content: recipeAggregate.value.document,
    onUpdate: ({ editor: currentEditor }) => {
        synchronizeAggregate(currentEditor.getJSON())
        blockCount.value = currentEditor.getJSON().content?.length ?? 0
        selectionRevision.value++
    },
    onSelectionUpdate: ({ editor: currentEditor }) => {
        selectionRevision.value++
        const { from, to, empty } = currentEditor.state.selection
        if (!empty && currentEditor.state.selection instanceof TextSelection) savedTextRange.value = { from, to }
    },
})

blockCount.value = editor.getJSON().content?.length ?? 0
onMounted(() => {
    window.addEventListener('pointermove', onWindowPointerMove)
    window.addEventListener('pointerup', onWindowPointerUp)
    editor.view.dom.addEventListener('focusin', onEditorFocusIn)
})
watch(title, (name) => {
    recipeAggregate.value.name = name
    saveStatus.value = 'Есть несохранённые изменения'
})

watch(preview, (isPreview) => editor.setEditable(!isPreview))

function synchronizeAggregate(document: JSONContent) {
    const ingredients = (document.content ?? [])
        .filter((node) => node.type === 'ingredientList')
        .flatMap((node) => (node.attrs?.ingredients as IngredientRowViewModel[] | undefined) ?? [])
        .filter((ingredient) => ingredient.name.trim())
    recipeAggregate.value.document = document
    recipeAggregate.value.description = editor.getText({ blockSeparator: '\n' }).trim()
    recipeAggregate.value.ingredients = ingredients.map<RecipeIngredientViewModel>((ingredient) => {
        return {
            name: ingredient.name.trim(),
            part: ingredient.part?.trim() || null,
            quantities: ingredient.quantities
                .map((quantity) => {
                    const amount = Number(quantity.amount.replace(',', '.'))
                    return amount > 0 && quantity.unit.trim()
                        ? { amount, unit: quantity.unit.trim() }
                        : null
                })
                .filter((quantity): quantity is { amount: number; unit: string } => quantity !== null),
        }
    })
    recipeAggregate.value.characteristics = []
    saveStatus.value = 'Есть несохранённые изменения'
}

function insertBlock(type: RecipeBlockType) {
    const { selection } = editor.state
    const position = selection.$to.depth > 0 ? selection.$to.after(1) : selection.to
    const node = editor.schema.nodeFromJSON(createRecipeBlockContent(type))
    const transaction = editor.state.tr.insert(position, node)
    if (node.isAtom && NodeSelection.isSelectable(node)) {
        transaction.setSelection(NodeSelection.create(transaction.doc, position))
    } else {
        transaction.setSelection(TextSelection.near(transaction.doc.resolve(position + 1)))
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
    if (event.button !== 0 || preview.value) return
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
    if (!dragType.value || preview.value) return
    dropPosition.value = editor.view.posAtCoords({ left: event.clientX, top: event.clientY })?.pos ?? editor.state.doc.content.size
}

function onCanvasDragLeave(event: DragEvent) {
    const currentTarget = event.currentTarget
    if (currentTarget instanceof HTMLElement && event.relatedTarget instanceof Node && currentTarget.contains(event.relatedTarget)) return
    dropPosition.value = null
}

function onCanvasDrop(event: DragEvent) {
    const type = dragType.value
    if (!type || preview.value) return
    const position = editor.view.posAtCoords({ left: event.clientX, top: event.clientY })?.pos ?? dropPosition.value ?? editor.state.doc.content.size
    const node = editor.schema.nodeFromJSON(createRecipeBlockContent(type))
    editor.view.dispatch(editor.state.tr.insert(position, node).scrollIntoView())
    finishPaletteDrag()
}

function isActive(name: string) {
    return editor.isActive(name)
}

function isTextAligned(alignment: 'left' | 'center' | 'right') {
    const activeIngredient = findActiveIngredient()
    if (activeIngredient) return activeIngredient.ingredient.textStyle?.textAlign === alignment
    return ['paragraph', 'heading', 'recipeParagraph']
        .some((type) => editor.isActive(type, { textAlign: alignment }))
}

function setTextAlignment(alignment: 'left' | 'center' | 'right') {
    if (findActiveIngredient()) {
        updateActiveIngredientStyle('textAlign', alignment)
        return
    }
    editor.chain().focus().setTextAlign(alignment).run()
}

function isTextMarkActive(mark: 'bold' | 'italic' | 'underline' | 'strike') {
    return findActiveIngredient()?.ingredient.textStyle?.[mark] ?? editor.isActive(mark)
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

function findActiveIngredient(): { position: number; attrs: Record<string, unknown>; ingredient: IngredientRowViewModel } | null {
    const id = activeIngredientId.value
    if (!id) return null
    const matches: { position: number; attrs: Record<string, unknown>; ingredient: IngredientRowViewModel }[] = []
    editor.state.doc.descendants((node, position) => {
        if (node.type.name !== 'ingredientList') return true
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
        const mark = field as 'bold' | 'italic' | 'underline' | 'strike'
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

function applyHeadingLevel(level: 1 | 2 | 3) {
    const fontSizeByLevel = { 1: 28, 2: 22, 3: 18 }
    fontSize.value = fontSizeByLevel[level]
    editor.chain().focus().setHeading({ level }).setMark('textStyle', { fontSize: `${fontSize.value}px` }).run()
}

function rememberBlockTitleTarget(event: PointerEvent) {
    const target = event.target
    const label = target instanceof Element ? target.closest('.recipe-paragraph__heading, .recipe-block__label') : null
    const block = label?.closest('.recipe-paragraph-block, .recipe-block')
    if (!block) {
        activeTitleBlockPosition.value = null
        return
    }

    let blockPosition: number | null = null
    editor.state.doc.descendants((node, position) => {
        if (['recipeParagraph', 'ingredientList'].includes(node.type.name)
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

function toggleMark(mark: 'bold' | 'italic' | 'underline' | 'strike') {
    if (updateActiveIngredientStyle(mark)) return
    const commands = editor.chain().focus()
    if (mark === 'bold') commands.toggleBold().run()
    else if (mark === 'italic') commands.toggleItalic().run()
    else if (mark === 'underline') commands.toggleUnderline().run()
    else commands.toggleStrike().run()
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
    if (position !== null && node && ['recipeParagraph', 'ingredientList'].includes(node.type.name)) {
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

function changeFontSize(delta: -1 | 1) {
    fontSize.value = Math.max(8, Math.min(72, fontSize.value + delta))
    applyFontSize(fontSize.value)
}

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

async function saveDraft() {
    synchronizeAggregate(editor.getJSON())
    recipeAggregate.value.name = title.value.trim()
    recipeAggregate.value.description ||= recipeAggregate.value.name

    if (!recipeAggregate.value.name) {
        saveStatus.value = 'Не удалось сохранить: укажите название'
        return
    }

    localStorage.setItem('recipe-draft', JSON.stringify({ title: recipeAggregate.value.name, document: recipeAggregate.value.document }))
    isSaving.value = true
    saveStatus.value = 'Сохраняем рецепт...'

    try {
        if (recipeAggregate.value.recipeId) {
            const result = await recipesStore.updateRecipe(recipeAggregate.value.recipeId, recipeAggregate.value)
            if (!result.isSuccess) throw new Error(result.error)
        } else {
            const result = await recipesStore.createRecipe(recipeAggregate.value)
            if (!result.isSuccess) throw new Error(result.error)
            const recipeId = result.data.id
            if (!recipeId) throw new Error('Сервер не вернул идентификатор рецепта')
            recipeAggregate.value.recipeId = recipeId
        }
        saveStatus.value = 'Рецепт сохранён'
    } catch (error) {
        saveStatus.value = `Не удалось сохранить: ${error instanceof Error ? error.message : 'ошибка сервера'}`
    } finally {
        isSaving.value = false
    }
}

onBeforeUnmount(() => {
    window.removeEventListener('pointermove', onWindowPointerMove)
    window.removeEventListener('pointerup', onWindowPointerUp)
    editor.view.dom.removeEventListener('focusin', onEditorFocusIn)
    editor.destroy()
})
</script>

<style scoped lang="scss">
.recipe-workspace {
    --ink: var(--p-text-color);
    --muted: var(--p-text-muted-color);
    --line: var(--p-content-border-color);
    --paper: var(--p-surface-0);
    --editor-radius: var(--p-border-radius-md, .5rem);
    min-height: calc(100vh - 4.25rem);
    padding: 1.25rem clamp(1rem, 3vw, 2.5rem) 3rem;
    background: var(--p-surface-ground);
    color: var(--ink);
}
.editor-layout { align-items: start; display: grid; gap: 1rem; grid-template-columns: 280px minmax(0, 920px); justify-content: center; max-width: 1500px; margin: auto; }
.editor-layout.is-sidebar-collapsed { grid-template-columns: 4.25rem minmax(0, 920px); }
@media (max-width: 850px) {
    .recipe-workspace { overflow-x: clip; padding: .75rem .65rem 2rem; }
    .editor-layout { display: block; }
}
</style>
