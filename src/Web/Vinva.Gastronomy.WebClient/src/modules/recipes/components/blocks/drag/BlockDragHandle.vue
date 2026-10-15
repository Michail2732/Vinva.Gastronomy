<template>
    <div ref="handle" class="block-drag-handle" contenteditable="false" title="Перетащить блок" @pointerdown="onPointerDown" />
</template>

<script setup lang="ts">
import { ref } from 'vue'
import type { Editor } from '@tiptap/core'
import { getBlockDragController } from './blockDragPlugin'

const props = defineProps<{ editor: Editor }>()

const handle = ref<HTMLElement | null>(null)

function onPointerDown(event: PointerEvent) {
    const element = handle.value
    if (!element) return
    const blockEl = element.closest<HTMLElement>('.recipe-block')
    if (!blockEl) return
    getBlockDragController(props.editor).start(event, blockEl)
}
</script>