<template>
    <NodeViewWrapper class="recipe-block recipe-image-block">
        <RecipeNodeToolbar :editable="editor.isEditable" :delete-node="deleteNode" delete-label="Удалить изображение">
            <label class="recipe-image-toolbar__width" title="Ширина изображения">
                <i class="pi pi-arrows-h" />
                <InputNumber :model-value="node.attrs.width" :min="25" :max="100" :step="5" suffix=" %" :use-grouping="false" aria-label="Ширина изображения в процентах" @update:model-value="updateWidth" />
            </label>
            <label class="recipe-image-toolbar__optimize" title="Оптимизировать загрузку изображения">
                <input type="checkbox" :checked="node.attrs.optimize" aria-label="Оптимизировать загрузку изображения" @change="updateOptimization" />
                <i class="pi pi-bolt" />
            </label>
        </RecipeNodeToolbar>
        <img
            v-if="node.attrs.src"
            :src="node.attrs.src"
            :alt="node.attrs.alt"
            :style="imageStyle"
            :loading="node.attrs.optimize ? 'lazy' : 'eager'"
            :decoding="node.attrs.optimize ? 'async' : 'sync'"
        />
        <div v-else-if="editor.isEditable" class="recipe-image-empty"><i class="pi pi-image" /><span>Добавьте ссылку на изображение</span></div>
        <input v-if="editor.isEditable" class="p-inputtext p-component recipe-image-url" type="url" :value="node.attrs.src" placeholder="https://..." aria-label="Ссылка на изображение" @input="updateSource" />
        <input v-if="editor.isEditable"
            class="recipe-image-caption"
            :value="node.attrs.caption"
            aria-label="Подпись к изображению"
            @input="updateCaption"
        />
        <figcaption v-else-if="node.attrs.src && node.attrs.caption" class="recipe-image-caption-view">{{ node.attrs.caption }}</figcaption>
    </NodeViewWrapper>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { NodeViewWrapper } from '@tiptap/vue-3'
import InputNumber from 'primevue/inputnumber'
import RecipeNodeToolbar from './recipeNodeToolbar.vue'

const props = defineProps<{
    editor: { isEditable: boolean }
    node: { attrs: { src: string; alt: string; caption: string; width: number; optimize: boolean } }
    updateAttributes: (attrs: Record<string, string | number | boolean>) => void
    deleteNode: () => void
}>()

const imageStyle = computed(() => ({ width: `${Math.min(100, Math.max(25, props.node.attrs.width ?? 100))}%` }))

function updateSource(event: Event) {
    props.updateAttributes({ src: (event.target as HTMLInputElement).value })
}

function updateWidth(value: number | null) {
    if (value !== null) props.updateAttributes({ width: Math.min(100, Math.max(25, value)) })
}

function updateOptimization(event: Event) {
    props.updateAttributes({ optimize: (event.target as HTMLInputElement).checked })
}

function updateCaption(event: Event) {
    props.updateAttributes({ caption: (event.target as HTMLInputElement).value })
}
</script>

<style scoped lang="scss">
.recipe-image-block { display: grid; gap: .6rem; margin: 2.5rem 0; position: relative; }
.recipe-image-toolbar__width, .recipe-image-toolbar__optimize { align-items: center; color: var(--p-text-muted-color); display: inline-flex; gap: .25rem; position: relative; }
.recipe-image-toolbar__width :deep(.p-inputnumber) { width: 4.5rem; }
.recipe-image-toolbar__width :deep(.p-inputnumber-input) { font-size: .7rem; min-width: 0; padding: .25rem .1rem; text-align: center; }
.recipe-image-toolbar__optimize { cursor: pointer; height: 1.65rem; justify-content: center; width: 1.65rem; }
.recipe-image-toolbar__optimize input { height: 1rem; margin: 0; width: 1rem; }
.recipe-image-block img { background: var(--p-surface-100); border-radius: var(--editor-radius); display: block; height: 245px; justify-self: center; max-width: 100%; object-fit: cover; }
.recipe-image-empty { align-items: center; background: var(--p-surface-50); border: 1px dashed var(--p-content-border-color); border-radius: var(--editor-radius); color: var(--p-text-muted-color); display: flex; flex-direction: column; gap: .5rem; height: 180px; justify-content: center; }
.recipe-image-empty i { color: var(--p-primary-color); font-size: 1.5rem; }
.recipe-image-url { width: 100%; }
.recipe-image-caption { background: transparent; border: 0; border-bottom: 1px solid var(--p-content-border-color); color: var(--p-text-muted-color); display: block; font-size: .72rem; margin: .65rem auto 0; outline: none; padding: .35rem; text-align: center; width: 100%; }
.recipe-image-caption-view { color: var(--p-text-muted-color); font-size: .78rem; text-align: center; }
@media (max-width: 850px) {
    .recipe-image-block { margin: 1.5rem 0; }
    .recipe-image-block img { height: 180px; }
}
</style>
