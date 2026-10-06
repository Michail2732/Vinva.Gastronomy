<template>
    <NodeViewWrapper class="recipe-block recipe-section" :class="`recipe-section--${node.type.name}`">
        <RecipeNodeToolbar :editable="editor.isEditable" :delete-node="deleteNode" delete-label="Удалить блок" />
        <button v-if="node.type.name === 'recipeToggle'" class="recipe-section__toggle" type="button" @mousedown.prevent @click="collapsed = !collapsed">
            <i :class="collapsed ? 'pi pi-chevron-right' : 'pi pi-chevron-down'" />
            <span>Подробности</span>
        </button>
        <div v-else class="recipe-section__label"><i class="pi pi-lightbulb" /> Совет</div>
        <NodeViewContent v-show="!collapsed" as="p" class="recipe-section__content" />
    </NodeViewWrapper>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { NodeViewContent, NodeViewWrapper } from '@tiptap/vue-3'
import RecipeNodeToolbar from './recipeNodeToolbar.vue'

defineProps<{
    editor: { isEditable: boolean }
    deleteNode: () => void
    node: { type: { name: string } }
}>()

const collapsed = ref(false)
</script>

<style scoped lang="scss">
.recipe-section { border-radius: var(--editor-radius); margin: 1rem 0; padding: .9rem 1rem; }
.recipe-section--recipeCallout { background: var(--p-primary-50); border: 1px solid var(--p-primary-100); border-left: 3px solid var(--p-primary-500); }
.recipe-section--recipeToggle { background: var(--p-surface-50); border: 1px solid var(--p-content-border-color); }
.recipe-section__label { color: var(--p-primary-700); font-size: .72rem; font-weight: 700; margin-bottom: .4rem; }
.recipe-section__label i { margin-right: .35rem; }
.recipe-section__toggle { align-items: center; background: transparent; border: 0; color: var(--p-text-color); cursor: pointer; display: flex; font: inherit; font-weight: 600; gap: .5rem; padding: 0; width: 100%; }
.recipe-section__toggle i { color: var(--p-text-muted-color); font-size: .75rem; }
.recipe-section__content { color: var(--p-text-color); line-height: 1.65; margin: .45rem 0 0; min-height: 1.5rem; outline: none; }
</style>