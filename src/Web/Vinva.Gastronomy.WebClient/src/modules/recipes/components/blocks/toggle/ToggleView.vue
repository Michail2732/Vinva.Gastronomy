<template>
    <NodeViewWrapper class="recipe-block recipe-toggle-block" :style="blockStyle">
        <BlockDragHandle v-if="isEditable" :editor="editor" />
        <RecipeNodeToolbar :editable="isEditable" :delete-node="deleteNode" delete-label="Удалить блок" />
        <div class="recipe-toggle__header" @click="toggle">
            <i :class="open ? 'pi pi-chevron-down' : 'pi pi-chevron-right'" />
            <input
                v-if="isEditable"
                class="recipe-toggle__title"
                :value="node.attrs.title || 'Раскрывающийся блок'"
                aria-label="Заголовок блока"
                @input="updateFromInput('title', $event)"
                @focus="focusBlock"
            />
            <strong v-else>{{ node.attrs.title || 'Раскрывающийся блок' }}</strong>
        </div>
        <NodeViewContent v-show="open" as="div" class="recipe-toggle__content" />
    </NodeViewWrapper>
</template>

<script setup lang="ts">
import { computed, inject, ref } from 'vue'
import type { Ref } from 'vue'
const isEditable = inject<Ref<boolean>>('editorEditable', ref(true))
import type { Editor } from '@tiptap/core'
import { NodeViewContent, NodeViewWrapper } from '@tiptap/vue-3'
import { NodeSelection } from '@tiptap/pm/state'
import RecipeNodeToolbar from '../../editor/recipeNodeToolbar.vue'
import BlockDragHandle from '../drag/BlockDragHandle.vue'

const props = defineProps<{
    editor: Editor
    getPos: () => number | undefined
    deleteNode: () => void
    node: { attrs: { title?: string | null; titleColor?: string | null; backgroundColor?: string | null; accentColor?: string | null; showAccent?: boolean } }
    updateAttributes: (attributes: Record<string, unknown>) => void
}>()

const open = ref(true)

const blockStyle = computed(() => ({
    backgroundColor: String(props.node.attrs.backgroundColor || 'var(--p-surface-0)'),
    borderColor: props.node.attrs.accentColor || 'var(--p-primary-color)',
    borderStyle: 'solid',
    borderWidth: '1px',
    borderRadius: '.5rem',
}))

function toggle() {
    open.value = !open.value
}

function updateFromInput(field: string, event: Event) {
    props.updateAttributes({ [field]: (event.target as HTMLInputElement).value })
}

function focusBlock() {
    const position = props.getPos()
    if (position === undefined) return
    const { state, dispatch } = props.editor.view
    const selection = NodeSelection.create(state.doc, position)
    dispatch(state.tr.setSelection(selection))
}
</script>

<style scoped lang="scss">
.recipe-toggle-block { background: var(--p-surface-0); margin: 1.25rem 0; padding: .75rem 1rem; position: relative; }
.recipe-toggle__header { align-items: center; color: var(--p-primary-700); display: flex; font-size: .9rem; font-weight: 700; gap: .45rem; }
.recipe-toggle__header > i { flex: 0 0 auto; }
.recipe-toggle__title { background: transparent; border: 0; color: inherit; font: inherit; min-width: 0; outline: none; width: 100%; }
.recipe-toggle__title:focus { box-shadow: 0 1px 0 var(--p-primary-color); }
.recipe-toggle__content { margin-top: .55rem; }
@media (max-width: 850px) {
    .recipe-toggle-block { padding: .7rem .75rem; }
}
</style>
