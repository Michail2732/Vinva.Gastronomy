<template>
    <NodeViewWrapper class="recipe-block recipe-image-block" :style="spacingStyle">
        <BlockDragHandle v-if="isEditable" :editor="editor" />
        <RecipeNodeToolbar :editable="isEditable" :delete-node="deleteNode" delete-label="Удалить изображение">
            <Button icon="pi pi-expand" text severity="secondary" aria-label="Размер изображения" title="Размер" @mousedown.prevent @click="toggleSizePopover" />
            <Button icon="pi pi-window-maximize" text severity="secondary" aria-label="Масштабирование изображения" title="Масштабирование" @mousedown.prevent @click="toggleStretchPopover" />
            <RecipeBlockSpacing :attrs="node.attrs" :update-attributes="updateAttributes" />
        </RecipeNodeToolbar>
        <Popover ref="sizePopover">
            <div class="recipe-image-settings" aria-label="Размер изображения">
                <label class="recipe-image-settings__row">
                    <span>Ширина</span>
                    <InputNumber :model-value="node.attrs.width" :min="24" :max="2400" :step="10" :use-grouping="false" suffix=" px" placeholder="Авто" aria-label="Ширина изображения в пикселях" @update:model-value="updateWidth" />
                </label>
                <label class="recipe-image-settings__row">
                    <span>Высота</span>
                    <InputNumber :model-value="node.attrs.height" :min="24" :max="2400" :step="10" :use-grouping="false" suffix=" px" placeholder="Авто" aria-label="Высота изображения в пикселях" @update:model-value="updateHeight" />
                </label>
                <Button class="recipe-image-settings__reset" icon="pi pi-undo" text severity="secondary" label="Сбросить размер" size="small" @mousedown.prevent @click="resetSize" />
            </div>
        </Popover>
        <Popover ref="stretchPopover">
            <div class="recipe-image-settings" aria-label="Масштабирование изображения">
                <span class="recipe-image-settings__label">Масштабирование</span>
                <SelectButton :model-value="node.attrs.stretch" :options="stretchOptions" option-label="label" option-value="value" :allow-empty="false" aria-label="Режим масштабирования" @update:model-value="updateStretch" />
            </div>
        </Popover>
        <img
            v-if="node.attrs.src"
            :src="node.attrs.src"
            :alt="node.attrs.alt"
            :style="imageStyle"
            :loading="node.attrs.optimize ? 'lazy' : 'eager'"
            :decoding="node.attrs.optimize ? 'async' : 'sync'"
        />
        <div v-else-if="isEditable" class="recipe-image-empty"><i class="pi pi-image" /><span>Добавьте ссылку на изображение</span></div>
        <input v-if="isEditable" class="p-inputtext p-component recipe-image-url" type="url" :value="node.attrs.src" placeholder="https://..." aria-label="Ссылка на изображение" @input="updateSource" />
        <input v-if="isEditable"
            class="recipe-image-caption"
            :value="node.attrs.caption"
            aria-label="Подпись к изображению"
            @input="updateCaption"
        />
        <figcaption v-else-if="node.attrs.src && node.attrs.caption" class="recipe-image-caption-view">{{ node.attrs.caption }}</figcaption>
    </NodeViewWrapper>
</template>

<script setup lang="ts">
import { computed, inject, ref } from 'vue'
import type { Ref } from 'vue'
import type { Editor } from '@tiptap/core'
const isEditable = inject<Ref<boolean>>('editorEditable', ref(true))
import { NodeViewWrapper } from '@tiptap/vue-3'
import Popover from 'primevue/popover'
import RecipeNodeToolbar from '../../editor/recipeNodeToolbar.vue'
import RecipeBlockSpacing from '../../editor/recipeBlockSpacing.vue'
import { blockMarginStyle } from '../../shared/margins'
import BlockDragHandle from '../drag/BlockDragHandle.vue'

const stretchOptions = [
    { label: 'Uniform', value: 'uniform' },
    { label: 'Fill', value: 'fill' },
    { label: 'UniformToFill', value: 'uniformToFill' },
]

const stretchObjectFit: Record<string, string> = {
    uniform: 'contain',
    fill: 'fill',
    uniformToFill: 'cover',
}

const props = defineProps<{
    editor: Editor
    node: { attrs: { src: string; alt: string; caption: string; width: number | null; height: number | null; stretch: string; optimize: boolean } }
    updateAttributes: (attrs: Record<string, string | number | boolean | null>) => void
    deleteNode: () => void
}>()

const imageStyle = computed(() => {
    const { width, height, stretch } = props.node.attrs
    const style: Record<string, string> = {
        width: width ? `${Math.max(1, Math.round(width))}px` : '100%',
        objectFit: stretchObjectFit[stretch] ?? 'contain',
    }
    if (height) style.height = `${Math.max(1, Math.round(height))}px`
    return style
})

const spacingStyle = computed(() => blockMarginStyle(props.node.attrs as unknown as Record<string, unknown>))

const sizePopover = ref<{ toggle: (event: Event) => void; hide: () => void } | null>(null)
const stretchPopover = ref<{ toggle: (event: Event) => void; hide: () => void } | null>(null)

function toggleSizePopover(event: MouseEvent) {
    sizePopover.value?.toggle(event)
}

function toggleStretchPopover(event: MouseEvent) {
    stretchPopover.value?.toggle(event)
}

function updateSource(event: Event) {
    props.updateAttributes({ src: (event.target as HTMLInputElement).value })
}

function updateWidth(value: number | null) {
    props.updateAttributes({ width: value })
}

function updateHeight(value: number | null) {
    props.updateAttributes({ height: value })
}

function updateStretch(value: string) {
    props.updateAttributes({ stretch: value })
}

function resetSize() {
    props.updateAttributes({ width: null, height: null })
}

function updateCaption(event: Event) {
    props.updateAttributes({ caption: (event.target as HTMLInputElement).value })
}
</script>

<style scoped lang="scss">
.recipe-image-block { display: grid; gap: .6rem; margin: 2.5rem 0; position: relative; }
.recipe-image-block img { background: var(--p-surface-100); border-radius: var(--p-border-radius-md, .5rem); display: block; justify-self: center; max-width: 100%; }
.recipe-image-settings { display: grid; gap: .55rem; min-width: 12rem; }
.recipe-image-settings__row { align-items: center; color: var(--p-text-muted-color); display: flex; font-size: .75rem; gap: .75rem; justify-content: space-between; }
.recipe-image-settings__row :deep(.p-inputnumber) { width: 6rem; }
.recipe-image-settings__row :deep(.p-inputnumber-input) { font-size: .72rem; min-width: 0; padding: .3rem .15rem; text-align: center; }
.recipe-image-settings__label { color: var(--p-text-muted-color); font-size: .75rem; font-weight: 600; }
.recipe-image-settings__reset { justify-content: flex-start; width: 100%; }
.recipe-image-settings :deep(.p-selectbutton) { width: 100%; }
.recipe-image-settings :deep(.p-selectbutton .p-togglebutton) { font-size: .72rem; padding: .3rem .4rem; }
.recipe-image-empty { align-items: center; background: var(--p-surface-50); border: 1px dashed var(--p-content-border-color); border-radius: var(--p-border-radius-md, .5rem); color: var(--p-text-muted-color); display: flex; flex-direction: column; gap: .5rem; height: 180px; justify-content: center; }
.recipe-image-empty i { color: var(--p-primary-color); font-size: 1.5rem; }
.recipe-image-url { width: 100%; }
.recipe-image-caption { background: transparent; border: 0; border-bottom: 1px solid var(--p-content-border-color); color: var(--p-text-muted-color); display: block; font-size: .72rem; margin: .65rem auto 0; outline: none; padding: .35rem; text-align: center; width: 100%; }
.recipe-image-caption-view { color: var(--p-text-muted-color); font-size: .78rem; text-align: center; }
@media (max-width: 850px) {
    .recipe-image-block { margin: 1.5rem 0; }
}
</style>
