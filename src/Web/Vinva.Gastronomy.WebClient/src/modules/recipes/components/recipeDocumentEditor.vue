<template>
    <section class="recipe-workspace">
        <div class="editor-layout" :class="{ 'is-sidebar-collapsed': sidebarCollapsed }">
            <RecipeEditorHeader
                v-model:title="title"
                v-model:preview="preview"
                :save-status="saveStatus"
                :save-status-icon="saveStatusIcon"
                :is-saving="isSaving"
                @save="saveDraft(editor)"
            />

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
                    :font-background-color="fontBackgroundColor"
                    :font-family="fontFamily"
                    :font-families="fontFamilies"
                    :can-move-block="canMoveBlock"
                    :is-text-aligned="isTextAligned"
                    :is-text-mark-active="isTextMarkActive"
                    @update:font-size="onFontSizeUpdate"
                    @update:font-color="onFontColorUpdate"
                    @update:font-background-color="onFontBackgroundColorUpdate"
                    @update:font-family="onFontFamilyUpdate"
                    @toggle-mark="toggleMark"
                    @align="setTextAlignment"
                    @font-step="changeFontSize"
                    @move-block="moveBlock"
                    @heading-level="applyHeadingLevel"
                />
                <RecipeEditorCanvas
                    :editor="editor"
                    :preview="preview"
                    :editable="isEditable"
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
import { onBeforeUnmount, onMounted, ref, watch } from 'vue'
import type { JSONContent } from '@tiptap/core'
import { createRecipeEditor, rememberTextSelection } from '../composables/recipeEditorFactory'
import { useRecipeAggregate } from '../composables/useRecipeAggregate'
import { useBlockPalette } from '../composables/useBlockPalette'
import { useBlockInsertion } from '../composables/useBlockInsertion'
import { useTextFormatting } from '../composables/useTextFormatting'
import { useBlockMovement } from '../composables/useBlockMovement'
import { normalizeRecipeDocument, sampleRecipeDocument } from '../recipeDocumentModel'
import RecipeBlockPalette from './editor/recipeBlockPalette.vue'
import RecipeTextToolbar from './editor/recipeTextToolbar.vue'
import RecipeEditorCanvas from './editor/recipeEditorCanvas.vue'
import RecipeEditorHeader from './editor/recipeEditorHeader.vue'

const props = withDefaults(defineProps<{
    initialTitle?: string
    initialDocument?: JSONContent
}>(), {
    initialTitle: 'Тыквенная паста с шалфеем',
})

const title = ref(props.initialTitle)
const preview = ref(false)
const isEditable = ref(true)
watch(preview, (isPreview) => {
    isEditable.value = !isPreview
    editor.setEditable(!isPreview)
})

const savedTextRange = ref<{ from: number; to: number } | null>(null)
const selectionRevision = ref(0)

const {
    recipeAggregate,
    isSaving,
    saveStatus,
    saveStatusIcon,
    synchronizeAggregate,
    saveDraft,
} = useRecipeAggregate({
    title,
    initialDocument: props.initialDocument ?? sampleRecipeDocument,
    normalize: normalizeRecipeDocument,
})

const {
    sidebarCollapsed,
    blockSearch,
    activeCategory,
    blockCount,
    blockCategories,
    visibleBlockGroups,
} = useBlockPalette()

const editor = createRecipeEditor({
    content: recipeAggregate.value.document,
    onUpdate: (currentEditor) => {
        synchronizeAggregate(currentEditor.getJSON(), currentEditor)
        blockCount.value = currentEditor.getJSON().content?.length ?? 0
        selectionRevision.value++
    },
    onSelectionUpdate: (currentEditor) => {
        selectionRevision.value++
        savedTextRange.value = rememberTextSelection(currentEditor) ?? savedTextRange.value
    },
})

blockCount.value = editor.getJSON().content?.length ?? 0

const {
    dragType,
    onPaletteClick,
    onPalettePointerDown,
    onWindowPointerMove,
    onWindowPointerUp,
    onCanvasDragOver,
    onCanvasDragLeave,
    onCanvasDrop,
} = useBlockInsertion(editor)

const {
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
} = useTextFormatting(editor, savedTextRange)

const { canMoveBlock, moveBlock } = useBlockMovement(editor, selectionRevision)

watch(title, (name) => {
    recipeAggregate.value.name = name
    saveStatus.value = 'Есть несохранённые изменения'
})

onMounted(() => {
    window.addEventListener('pointermove', onWindowPointerMove)
    window.addEventListener('pointerup', onWindowPointerUp)
    editor.view.dom.addEventListener('focusin', onEditorFocusIn)
})

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
.editor-layout > .recipe-workspace__header { grid-column: 1 / -1; }
.editor-layout.is-sidebar-collapsed { grid-template-columns: 4.25rem minmax(0, 920px); }
@media (max-width: 850px) {
    .recipe-workspace { overflow-x: clip; padding: .75rem .65rem 2rem; }
    .editor-layout { display: block; }
}
</style>
