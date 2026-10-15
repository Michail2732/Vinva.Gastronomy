<template>
	<NodeViewWrapper class="recipe-block recipe-gallery-block">
		<BlockDragHandle v-if="isEditable" :editor="editor" />
		<RecipeNodeToolbar :editable="isEditable" :delete-node="deleteNode" delete-label="Удалить галерею" />
		<div class="recipe-block__label"><i class="pi pi-images" /> Галерея изображений</div>
		<div class="gallery-grid">
			<figure v-for="(src, index) in images" :key="`${src}-${index}`" class="gallery-item">
				<img :src="src" :alt="`Кадр процесса ${index + 1}`" />
				<button v-if="isEditable" class="gallery-item__remove" type="button" :aria-label="`Удалить кадр ${index + 1}`" @click="removeImage(index)"><i class="pi pi-times" /></button>
			</figure>
			<div v-if="isEditable && !images.length" class="gallery-empty"><i class="pi pi-images" /><span>Добавьте изображения процесса</span></div>
		</div>
		<div v-if="isEditable" class="gallery-add">
			<input class="p-inputtext p-component" type="url" v-model="imageUrl" placeholder="https://..." aria-label="Ссылка на кадр галереи" @keydown.enter.prevent="addImage" />
			<button class="gallery-add__button" type="button" title="Добавить кадр" @click="addImage"><i class="pi pi-plus" /></button>
		</div>
	</NodeViewWrapper>
</template>

<script setup lang="ts">
import { computed, inject, ref } from 'vue'
import type { Ref } from 'vue'
import type { Editor } from '@tiptap/core'
const isEditable = inject<Ref<boolean>>('editorEditable', ref(true))
import { NodeViewWrapper } from '@tiptap/vue-3'
import RecipeNodeToolbar from '../../editor/recipeNodeToolbar.vue'
import BlockDragHandle from '../drag/BlockDragHandle.vue'

const props = defineProps<{
	editor: Editor
	node: { attrs: { images: string[] } }
	updateAttributes: (attrs: Record<string, string[]>) => void
	deleteNode: () => void
}>()

const images = computed(() => props.node.attrs.images ?? [])
const imageUrl = ref('')

function addImage() {
	const value = imageUrl.value.trim()
	if (!value) return
	props.updateAttributes({ images: [...images.value, value] })
	imageUrl.value = ''
}

function removeImage(index: number) {
	props.updateAttributes({ images: images.value.filter((_, itemIndex) => itemIndex !== index) })
}
</script>

<style scoped lang="scss">
.recipe-gallery-block { margin: 2rem 0; position: relative; }
.recipe-block__label { color: var(--p-primary-color); font-size: .67rem; font-weight: 700; letter-spacing: .08em; margin-bottom: .55rem; text-transform: uppercase; }
.recipe-block__label i { margin-right: .35rem; }
.gallery-grid { display: grid; gap: .6rem; grid-template-columns: repeat(3, 1fr); }
.gallery-grid img { background: var(--p-surface-100); border-radius: var(--p-border-radius-md, .5rem); height: 135px; object-fit: cover; width: 100%; }
.gallery-item { margin: 0; position: relative; }
.gallery-item__remove { align-items: center; background: var(--p-surface-0); border: 1px solid var(--p-content-border-color); border-radius: 50%; color: var(--p-text-muted-color); cursor: pointer; display: flex; height: 1.75rem; justify-content: center; opacity: 0; position: absolute; right: .4rem; top: .4rem; width: 1.75rem; }
.gallery-item:hover .gallery-item__remove { opacity: 1; }
.gallery-empty { align-items: center; background: var(--p-surface-50); border: 1px dashed var(--p-content-border-color); border-radius: var(--p-border-radius-md, .5rem); color: var(--p-text-muted-color); display: flex; flex-direction: column; gap: .45rem; grid-column: 1 / -1; justify-content: center; min-height: 120px; }
.gallery-empty i { color: var(--p-primary-color); font-size: 1.25rem; }
.gallery-add { display: flex; gap: .5rem; margin-top: .6rem; }
.gallery-add input { flex: 1; min-width: 0; }
.gallery-add__button { align-items: center; background: var(--p-primary-color); border: 1px solid var(--p-primary-color); border-radius: var(--p-button-border-radius); color: var(--p-primary-contrast-color); cursor: pointer; display: inline-flex; justify-content: center; min-width: 2.5rem; }
@media (max-width: 850px) {
	.gallery-grid { grid-template-columns: repeat(2, minmax(0, 1fr)); }
}
</style>
