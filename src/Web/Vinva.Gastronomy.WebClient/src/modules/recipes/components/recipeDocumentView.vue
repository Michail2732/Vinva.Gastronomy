<template>
    <article class="recipe-document-view">
        <header class="recipe-document-view__header">
            <p class="eyebrow">Recipe / {{ recipeId }}</p>
            <h1>{{ title }}</h1>
            <p v-if="description" class="recipe-document-view__description">{{ description }}</p>
        </header>
        <div class="recipe-document-view__body">
            <EditorContent :editor="editor" class="recipe-document-view__content" />
        </div>
    </article>
</template>

<script setup lang="ts">
import { onBeforeUnmount, provide, ref } from 'vue'
import type { JSONContent } from '@tiptap/core'
import { Editor, EditorContent } from '@tiptap/vue-3'
import StarterKit from '@tiptap/starter-kit'
import Image from '@tiptap/extension-image'
import Underline from '@tiptap/extension-underline'
import Color from '@tiptap/extension-color'
import TextAlign from '@tiptap/extension-text-align'
import {
    RecipeMedia, RecipeToggle, IngredientList, GroupBox, TextBlock,
    RecipeDivider, ImageGallery, RecipeImage, RecipeTaskList, RecipeTaskItem,
    RecipeTable, RecipeTableRow, RecipeTableHeader, RecipeTableCell, RecipeTextStyle,
} from '../recipeBlockExtensions'
import { normalizeRecipeDocument, sampleRecipeDocument } from '../recipeDocumentModel'

const props = defineProps<{
    recipeId?: string
    title: string
    description?: string | null
    document?: JSONContent
}>()

provide('editorEditable', ref(false))

const editor = new Editor({
    editable: false,
    extensions: [
        StarterKit.configure({ horizontalRule: false, blockquote: false, underline: false, trailingNode: false }),
        Image,
        Underline,
        RecipeTextStyle,
        Color,
        TextAlign.configure({ types: ['paragraph', 'heading', 'groupBox', 'textBlock'] }),
        RecipeTaskList,
        RecipeTaskItem,
        RecipeTable,
        RecipeTableRow,
        RecipeTableHeader,
        RecipeTableCell,
        RecipeToggle,
        RecipeMedia,
        IngredientList,
        GroupBox,
        TextBlock,
        RecipeDivider,
        RecipeImage,
        ImageGallery,
    ],
    content: normalizeRecipeDocument(props.document ?? sampleRecipeDocument),
})

onBeforeUnmount(() => editor.destroy())
</script>

<style scoped lang="scss">
.recipe-document-view {
    --editor-radius: .5rem;
    background: var(--p-surface-ground);
    color: var(--p-text-color);
    min-height: calc(100vh - 4.25rem);
    padding: 2rem clamp(1rem, 4vw, 4rem) 4rem;
}
.recipe-document-view__header,
.recipe-document-view__body {
    background: var(--p-surface-0);
    border: 1px solid var(--p-content-border-color);
    border-radius: var(--editor-radius);
    margin: 0 auto;
    max-width: 820px;
}
.recipe-document-view__header { margin-bottom: .75rem; padding: 2rem clamp(1.25rem, 6vw, 4rem); }
.eyebrow { color: var(--p-primary-color); font-size: .7rem; font-weight: 700; letter-spacing: .08em; margin: 0 0 .75rem; text-transform: uppercase; }
h1 { font-size: clamp(1.8rem, 4vw, 3rem); line-height: 1.1; margin: 0; }
.recipe-document-view__description { color: var(--p-text-muted-color); line-height: 1.6; margin: 1rem 0 0; }
.recipe-document-view__body { padding: clamp(1.25rem, 6vw, 4rem); }
.recipe-document-view__content :deep(.ProseMirror) { outline: none; }
.recipe-document-view__content :deep(.ProseMirror > p) { color: var(--p-text-color); font-size: 1rem; line-height: 1.7; margin: 0 0 1.5rem; }
.recipe-document-view__content :deep(.ProseMirror table) { background: var(--recipe-table-background, transparent); border-collapse: collapse; margin: 1rem 0; table-layout: fixed; width: 100%; }
.recipe-document-view__content :deep(.ProseMirror td), .recipe-document-view__content :deep(.ProseMirror th) { border: var(--recipe-grid-width, 1px) solid var(--recipe-grid-color, var(--p-content-border-color)); min-width: 5rem; padding: .5rem; vertical-align: top; }
.recipe-document-view__content :deep(.ProseMirror td) { background-color: var(--recipe-table-background, transparent); }
.recipe-document-view__content :deep(.ProseMirror th) { background-color: var(--recipe-table-background, var(--p-surface-100)); }
@media (max-width: 850px) {
    .recipe-document-view { padding: 1rem .75rem 2rem; }
    .recipe-document-view__header { padding: 1.25rem; }
    .recipe-document-view__body { padding: 1.25rem; }
}
</style>
