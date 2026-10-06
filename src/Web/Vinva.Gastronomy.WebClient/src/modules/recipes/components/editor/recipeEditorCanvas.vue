<template>
    <div class="paper-shell" :class="{ 'is-preview': preview, 'is-drop-target': isDropTarget }" @pointerdown.capture="emit('pointerdown', $event)" @dragover.prevent="emit('dragover', $event)" @drop.prevent="emit('drop', $event)" @dragleave="emit('dragleave', $event)">
        <div v-if="isDropTarget" class="canvas-drop-hint"><i class="pi pi-download" /> Отпустите, чтобы вставить блок</div>
        <div v-if="preview" class="preview-label"><i class="pi pi-eye" /> Режим предпросмотра</div>
        <EditorContent :key="preview ? 'preview' : 'edit'" :editor="editor" class="recipe-editor" />
    </div>
</template>

<script setup lang="ts">
import type { Editor } from '@tiptap/vue-3'
import { EditorContent } from '@tiptap/vue-3'

defineProps<{
    editor: Editor
    preview: boolean
    isDropTarget: boolean
}>()

const emit = defineEmits<{
    pointerdown: [event: PointerEvent]
    dragover: [event: DragEvent]
    drop: [event: DragEvent]
    dragleave: [event: DragEvent]
}>()
</script>

<style scoped lang="scss">
.paper-shell { background: var(--p-surface-0); border: 1px solid var(--p-content-border-color); border-radius: var(--p-border-radius-md, .5rem); margin-top: .65rem; min-height: 720px; padding: clamp(1.5rem, 4vw, 3.5rem) clamp(1.2rem, 5vw, 4.5rem); position: relative; }
.paper-shell.is-drop-target { outline: 2px dashed var(--p-primary-300); outline-offset: 3px; }
.canvas-drop-hint { align-items: center; background: var(--p-primary-50); border: 1px dashed var(--p-primary-300); border-radius: var(--p-border-radius-md, .5rem); color: var(--p-primary-700); display: flex; font-size: .8rem; gap: .45rem; justify-content: center; margin: 0 auto 1rem; max-width: 20rem; padding: .55rem .8rem; pointer-events: none; }
.preview-label { color: var(--p-primary-color); font-size: .7rem; font-weight: 700; margin-bottom: 1.25rem; text-transform: uppercase; }
.recipe-editor :deep(.ProseMirror) { min-height: 600px; outline: none; }
.recipe-editor :deep(.ProseMirror > p) { color: var(--p-text-color); font-size: 1rem; line-height: 1.7; margin: 0 0 1.5rem; }
.recipe-editor :deep(.ProseMirror > .is-active-block) { outline: 2px solid color-mix(in srgb, var(--p-primary-color) 42%, transparent); outline-offset: 5px; }
.recipe-editor :deep(.ProseMirror > p.is-editor-empty:first-child::before) { color: var(--p-form-field-placeholder-color); content: attr(data-placeholder); float: left; height: 0; pointer-events: none; }
.recipe-editor :deep(.ProseMirror h1), .recipe-editor :deep(.ProseMirror h2), .recipe-editor :deep(.ProseMirror h3) { color: var(--p-text-color); line-height: 1.25; margin: 1.7rem 0 .65rem; }
.recipe-editor :deep(.ProseMirror h1) { font-size: 1.75rem; }
.recipe-editor :deep(.ProseMirror h2) { font-size: 1.35rem; }
.recipe-editor :deep(.ProseMirror h3) { font-size: 1.05rem; }
.recipe-editor :deep(.ProseMirror blockquote) { border-left: 3px solid var(--p-primary-300); color: var(--p-text-muted-color); margin: 1rem 0; padding: .25rem 0 .25rem 1rem; }
.recipe-editor :deep(.ProseMirror pre) { background: var(--p-surface-900); border-radius: var(--p-border-radius-md, .5rem); color: var(--p-surface-0); overflow-x: auto; padding: 1rem; }
.recipe-editor :deep(.ProseMirror table) { background: var(--recipe-table-background, transparent); border-collapse: collapse; margin: 1rem 0; table-layout: fixed; width: 100%; }
.recipe-editor :deep(.ProseMirror th), .recipe-editor :deep(.ProseMirror td) { border: var(--recipe-grid-width, 1px) solid var(--recipe-grid-color, var(--p-content-border-color)); min-width: 5rem; padding: .5rem; vertical-align: top; }
.recipe-editor :deep(.ProseMirror td.is-active-cell), .recipe-editor :deep(.ProseMirror th.is-active-cell) { box-shadow: inset 0 0 0 2px var(--p-primary-color); }
.recipe-editor :deep(.ProseMirror .selectedCell) { background-color: color-mix(in srgb, var(--p-primary-300) 38%, var(--p-surface-0)); }
.recipe-editor :deep(.ProseMirror .tableWrapper.is-active-table) { outline: 2px solid color-mix(in srgb, var(--p-primary-color) 42%, transparent); outline-offset: 3px; }
.recipe-editor :deep(.ProseMirror th) { background-color: var(--recipe-table-background, var(--p-surface-100)); font-weight: 700; }
.recipe-editor :deep(.ProseMirror td) { background-color: var(--recipe-table-background, transparent); }
.recipe-editor :deep(.ProseMirror ul[data-type='taskList']) { list-style: none; padding-left: .2rem; }
.recipe-editor :deep(.ProseMirror ul[data-type='taskList'] li) { align-items: flex-start; display: flex; gap: .45rem; }
.recipe-editor :deep(.ProseMirror ul[data-type='taskList'] li > label) { align-items: center; display: inline-flex; flex: 0 0 1.2rem; height: 1.7rem; justify-content: center; margin: 0; }
.recipe-editor :deep(.ProseMirror ul[data-type='taskList'] li > div) { flex: 1; min-width: 0; }
.recipe-editor :deep(.ProseMirror ul[data-type='taskList'] li > div > p) { line-height: 1.7; margin: 0; min-height: 1.7rem; }
.recipe-editor :deep(.ProseMirror ul[data-type='taskList'] input) { accent-color: var(--p-primary-color); margin: 0; }
@media (max-width: 850px) {
    .paper-shell { min-height: 600px; padding: 1.6rem 1rem; }
}
</style>