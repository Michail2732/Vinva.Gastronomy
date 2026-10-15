<template>
    <div class="paper-shell" :class="{ 'is-preview': preview, 'is-drop-target': isDropTarget }" @pointerdown.capture="emit('pointerdown', $event)" @dragover.prevent="emit('dragover', $event)" @drop.prevent="emit('drop', $event)" @dragleave="emit('dragleave', $event)">
        <div v-if="isDropTarget" class="canvas-drop-hint"><i class="pi pi-download" /> Отпустите, чтобы вставить блок</div>
        <div v-if="preview" class="preview-label"><i class="pi pi-eye" /> Режим предпросмотра</div>
        <EditorContent :editor="editor" class="recipe-editor" />
    </div>
</template>

<script setup lang="ts">
import type { Editor } from '@tiptap/vue-3'
import { EditorContent } from '@tiptap/vue-3'
import { computed, provide } from 'vue'

const props = defineProps<{
    editor: Editor
    preview: boolean
    editable: boolean
    isDropTarget: boolean
}>()

const editableRef = computed(() => props.editable)
provide('editorEditable', editableRef)

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
.recipe-editor :deep(.ProseMirror > .text-block.is-editor-empty:first-child)::before, .recipe-editor :deep(.ProseMirror > .text-block.is-empty)::before { color: var(--p-form-field-placeholder-color); content: attr(data-placeholder); left: 0; pointer-events: none; position: absolute; top: 0; }
.recipe-editor :deep(.ProseMirror h1), .recipe-editor :deep(.ProseMirror h2), .recipe-editor :deep(.ProseMirror h3) { color: var(--p-text-color); line-height: 1.25; margin: 1.7rem 0 .65rem; }
.recipe-editor :deep(.ProseMirror h1) { font-size: 1.75rem; }
.recipe-editor :deep(.ProseMirror h2) { font-size: 1.35rem; }
.recipe-editor :deep(.ProseMirror h3) { font-size: 1.05rem; }
.recipe-editor :deep(.ProseMirror blockquote) { border-left: 3px solid var(--p-primary-300); color: var(--p-text-muted-color); margin: 1rem 0; padding: .25rem 0 .25rem 1rem; }
.recipe-editor :deep(.ProseMirror pre) { background: var(--p-surface-900); border-radius: var(--p-border-radius-md, .5rem); color: var(--p-surface-0); overflow-x: auto; padding: 1rem; }
.recipe-editor :deep(.ProseMirror table) { background: var(--recipe-table-background, transparent); border-collapse: collapse; margin: 1rem 0; table-layout: fixed; width: 100%; max-width: 100%; }
.recipe-editor :deep(.tableWrapper.recipe-table) { overflow-x: auto; max-width: 100%; }
.recipe-editor :deep(.ProseMirror th), .recipe-editor :deep(.ProseMirror td) { border: var(--recipe-grid-width, 1px) solid var(--recipe-grid-color, var(--p-content-border-color)); min-width: 5rem; padding: .5rem; position: relative; vertical-align: top; }
.recipe-editor :deep(.ProseMirror td.is-active-cell), .recipe-editor :deep(.ProseMirror th.is-active-cell) { box-shadow: inset 0 0 0 2px var(--p-primary-color); }
.recipe-editor :deep(.ProseMirror .selectedCell) { background-color: color-mix(in srgb, var(--p-primary-300) 38%, var(--p-surface-0)); }
.recipe-editor :deep(.ProseMirror .column-resize-handle) { background: color-mix(in srgb, var(--p-primary-color) 55%, transparent); border-radius: 2px; bottom: calc(-1 * var(--recipe-grid-width, 1px)); pointer-events: none; position: absolute; right: calc(-1 * (var(--recipe-grid-width, 1px) / 2) - 2px); top: calc(-1 * var(--recipe-grid-width, 1px)); width: 4px; z-index: 20; }
.recipe-editor :deep(.ProseMirror .column-resize-dragging .column-resize-handle) { background: var(--p-primary-color); box-shadow: 0 0 0 1px color-mix(in srgb, var(--p-primary-color) 55%, transparent), 0 0 10px color-mix(in srgb, var(--p-primary-color) 65%, transparent); }
.recipe-editor :deep(.ProseMirror.resize-cursor) { cursor: col-resize; }
.recipe-editor :deep(.ProseMirror td.column-resize-hover), .recipe-editor :deep(.ProseMirror th.column-resize-hover) { box-shadow: inset -3px 0 0 0 color-mix(in srgb, var(--p-primary-color) 55%, transparent); }
.recipe-editor :deep(.ProseMirror td.column-resize-dragging), .recipe-editor :deep(.ProseMirror th.column-resize-dragging) { box-shadow: inset 0 0 0 1px var(--p-primary-color); }
.recipe-editor :deep(.ProseMirror td.has-cell-border::after), .recipe-editor :deep(.ProseMirror th.has-cell-border::after) { border-style: solid; border-color: var(--recipe-cell-border-color, var(--recipe-grid-color, var(--p-content-border-color))); border-width: var(--recipe-cell-border-top, 0) var(--recipe-cell-border-right, 0) var(--recipe-cell-border-bottom, 0) var(--recipe-cell-border-left, 0); content: ''; inset: calc(-1.5 * var(--recipe-grid-width, 1px)); pointer-events: none; position: absolute; }
.recipe-editor :deep(.ProseMirror .tableWrapper.is-active-table) { outline: 2px solid color-mix(in srgb, var(--p-primary-color) 42%, transparent); outline-offset: 3px; }
.recipe-editor :deep(.ProseMirror .block-drag-target) { outline: 2px solid color-mix(in srgb, var(--p-primary-color) 42%, transparent); outline-offset: 5px; position: relative; }
.recipe-editor :deep(.ProseMirror .recipe-block) { position: relative; }
.recipe-editor :deep(.ProseMirror .block-drag-handle) { background-color: var(--p-surface-0); background-image: url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 24 24' fill='%2394a3b8'%3E%3Cpath d='M11 18c0 1.1-.9 2-2 2s-2-.9-2-2 .9-2 2-2 2 .9 2 2zm-2-8c-1.1 0-2 .9-2 2s.9 2 2 2 2-.9 2-2-.9-2-2-2zm0-6c-1.1 0-2 .9-2 2s.9 2 2 2 2-.9 2-2-.9-2-2-2zm6 4c1.1 0 2-.9 2-2s-.9-2-2-2-2 .9-2 2 .9 2 2 2zm0 2c-1.1 0-2 .9-2 2s.9 2 2 2 2-.9 2-2-.9-2-2-2zm0 6c-1.1 0-2 .9-2 2s.9 2 2 2 2-.9 2-2-.9-2-2-2z'/%3E%3C/svg%3E"); background-position: center; background-repeat: no-repeat; background-size: 14px; border: 1px solid var(--p-content-border-color); border-radius: 6px; cursor: grab; height: 24px; left: -32px; opacity: 0; position: absolute; top: 6px; touch-action: none; transition: opacity .15s ease; user-select: none; width: 24px; z-index: 4; }
.recipe-editor :deep(.ProseMirror .recipe-block:hover > .block-drag-handle) { opacity: 1; }
.recipe-editor :deep(.ProseMirror .block-drag-handle:active) { cursor: grabbing; }
@media (max-width: 850px) { .recipe-editor :deep(.ProseMirror .block-drag-handle) { left: 2px; top: 2px; } }


.recipe-editor :deep(.ProseMirror .block-dragging-node) { box-shadow: 0 14px 34px -14px color-mix(in srgb, var(--p-primary-color) 65%, transparent), 0 0 0 2px color-mix(in srgb, var(--p-primary-color) 55%, transparent); border-radius: 8px; opacity: .92; position: relative; transition: box-shadow .15s ease; z-index: 6; }
.recipe-editor :deep(.ProseMirror.block-dragging) { cursor: grabbing; user-select: none; }
.recipe-editor :deep(.ProseMirror.block-dragging *) { cursor: grabbing !important; }
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