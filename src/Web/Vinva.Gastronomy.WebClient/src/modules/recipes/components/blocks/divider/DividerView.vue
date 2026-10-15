<template>
    <NodeViewWrapper class="recipe-block recipe-divider-block">
        <BlockDragHandle v-if="isEditable" :editor="editor" />
        <hr />
    </NodeViewWrapper>
</template>

<script setup lang="ts">
import { inject, ref } from 'vue'
import type { Ref } from 'vue'
import type { Editor } from '@tiptap/core'
import { NodeViewWrapper } from '@tiptap/vue-3'
import BlockDragHandle from '../drag/BlockDragHandle.vue'

defineProps<{ editor: Editor }>()

const isEditable = inject<Ref<boolean>>('editorEditable', ref(true))
</script>

<style scoped lang="scss">
.recipe-divider-block { margin: 1.5rem 0; }
hr { border: 0; border-top: 2px solid var(--p-primary-color); margin: 0; opacity: .6; }
</style>
