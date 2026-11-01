<template>
	<NodeViewWrapper class="recipe-block recipe-gallery-block" :style="spacingStyle">
		<BlockDragHandle v-if="isEditable" :editor="editor" />
		<RecipeNodeToolbar :editable="isEditable" :delete-node="deleteNode" delete-label="Удалить галерею">
			<Button icon="pi pi-sliders-h" text severity="secondary" aria-label="Настройки галереи" title="Настройки галереи" @mousedown.prevent @click="toggleSettings" />
			<RecipeBlockSpacing :attrs="node.attrs" :update-attributes="updateAttributes" />
		</RecipeNodeToolbar>
		<Popover ref="settingsPopover">
			<div class="gallery-settings">
				<span class="gallery-settings__label">Остальные кадры</span>
				<SelectButton :model-value="layout" :options="layoutOptions" option-label="label" option-value="value" :allow-empty="false" aria-label="Отображение остальных кадров" @update:model-value="updateLayout" />
			</div>
		</Popover>
		<div v-if="images.length" class="gallery-carousel">
			<figure class="gallery-stage">
				<img :src="activeImage" :alt="`Кадр ${activeIndex + 1}`" />
				<template v-if="images.length > 1">
					<button class="gallery-stage__nav gallery-stage__nav--prev" type="button" aria-label="Предыдущий кадр" @click="prevImage"><i class="pi pi-chevron-left" /></button>
					<button class="gallery-stage__nav gallery-stage__nav--next" type="button" aria-label="Следующий кадр" @click="nextImage"><i class="pi pi-chevron-right" /></button>
					<span class="gallery-stage__counter">{{ activeIndex + 1 }} / {{ images.length }}</span>
				</template>
				<button v-if="isEditable" class="gallery-stage__remove" type="button" :aria-label="`Удалить кадр ${activeIndex + 1}`" @click="removeImage(activeIndex)"><i class="pi pi-times" /></button>
			</figure>
			<div v-if="otherImages.length" class="gallery-thumbs" :class="`gallery-thumbs--${layout}`">
				<button v-for="item in otherImages" :key="`${item.src}-${item.index}`" class="gallery-thumb" type="button" :aria-label="`Показать кадр ${item.index + 1}`" @click="selectImage(item.index)">
					<img :src="item.src" :alt="`Кадр ${item.index + 1}`" />
				</button>
			</div>
		</div>
		<div v-else-if="isEditable" class="gallery-empty"><i class="pi pi-images" /><span>Добавьте изображения процесса</span></div>
		<div v-if="isEditable" class="gallery-add">
			<input class="p-inputtext p-component" type="url" v-model="imageUrl" placeholder="https://..." aria-label="Ссылка на кадр галереи" @keydown.enter.prevent="addImage" />
			<button class="gallery-add__button" type="button" title="Добавить кадр" @click="addImage"><i class="pi pi-plus" /></button>
		</div>
	</NodeViewWrapper>
</template>

<script setup lang="ts">
import { computed, inject, ref, watch } from 'vue'
import type { Ref } from 'vue'
import type { Editor } from '@tiptap/core'
const isEditable = inject<Ref<boolean>>('editorEditable', ref(true))
import { NodeViewWrapper } from '@tiptap/vue-3'
import Popover from 'primevue/popover'
import RecipeNodeToolbar from '../../editor/recipeNodeToolbar.vue'
import RecipeBlockSpacing from '../../editor/recipeBlockSpacing.vue'
import { blockMarginStyle } from '../../shared/margins'
import BlockDragHandle from '../drag/BlockDragHandle.vue'

const props = defineProps<{
	editor: Editor
	node: { attrs: { images: string[]; layout?: string } }
	updateAttributes: (attrs: Record<string, unknown>) => void
	deleteNode: () => void
}>()

const spacingStyle = computed(() => blockMarginStyle(props.node.attrs as unknown as Record<string, unknown>))
const images = computed(() => props.node.attrs.images ?? [])
const layout = computed(() => (props.node.attrs.layout === 'slider' ? 'slider' : 'bottom'))
const imageUrl = ref('')
const currentIndex = ref(0)
const settingsPopover = ref<InstanceType<typeof Popover> | null>(null)

const layoutOptions = [
	{ label: 'Снизу', value: 'bottom' },
	{ label: 'Слайдер', value: 'slider' },
]

const activeIndex = computed(() => (images.value.length ? Math.min(Math.max(currentIndex.value, 0), images.value.length - 1) : 0))
const activeImage = computed(() => images.value[activeIndex.value] ?? '')
const otherImages = computed(() => images.value.map((src, index) => ({ src, index })).filter((item) => item.index !== activeIndex.value))

watch(
	() => images.value.length,
	(length) => {
		if (currentIndex.value > length - 1) currentIndex.value = Math.max(0, length - 1)
	},
)

function addImage() {
	const value = imageUrl.value.trim()
	if (!value) return
	props.updateAttributes({ images: [...images.value, value] })
	imageUrl.value = ''
}

function removeImage(index: number) {
	props.updateAttributes({ images: images.value.filter((_, itemIndex) => itemIndex !== index) })
}

function selectImage(index: number) {
	currentIndex.value = index
}

function prevImage() {
	const count = images.value.length
	if (count < 2) return
	currentIndex.value = (activeIndex.value - 1 + count) % count
}

function nextImage() {
	const count = images.value.length
	if (count < 2) return
	currentIndex.value = (activeIndex.value + 1) % count
}

function toggleSettings(event: Event) {
	settingsPopover.value?.toggle(event)
}

function updateLayout(value: unknown) {
	props.updateAttributes({ layout: value === 'slider' ? 'slider' : 'bottom' })
}
</script>

<style scoped lang="scss">
.recipe-gallery-block { margin: 2rem 0; position: relative; }
.gallery-carousel { display: grid; gap: .55rem; }
.gallery-stage { background: var(--p-surface-100); border-radius: var(--p-border-radius-md, .5rem); margin: 0; overflow: hidden; position: relative; }
.gallery-stage > img { display: block; height: clamp(180px, 34vw, 340px); object-fit: cover; width: 100%; }
.gallery-stage__nav { align-items: center; background: color-mix(in srgb, var(--p-surface-0) 82%, transparent); border: 0; border-radius: 50%; color: var(--p-text-color); cursor: pointer; display: flex; height: 2.25rem; justify-content: center; position: absolute; top: 50%; transform: translateY(-50%); transition: background var(--p-transition-duration, .2s); width: 2.25rem; z-index: 1; }
.gallery-stage__nav:hover { background: var(--p-surface-0); }
.gallery-stage__nav--prev { left: .6rem; }
.gallery-stage__nav--next { right: .6rem; }
.gallery-stage__counter { background: color-mix(in srgb, var(--p-surface-900) 62%, transparent); border-radius: 999px; bottom: .6rem; color: #fff; font-size: .68rem; left: .6rem; padding: .15rem .5rem; position: absolute; }
.gallery-stage__remove { align-items: center; background: color-mix(in srgb, var(--p-surface-0) 85%, transparent); border: 1px solid var(--p-content-border-color); border-radius: 50%; color: var(--p-text-muted-color); cursor: pointer; display: flex; height: 1.9rem; justify-content: center; position: absolute; right: .6rem; top: .6rem; width: 1.9rem; z-index: 1; }
.gallery-stage__remove:hover { background: var(--p-red-50); color: var(--p-red-600); }
.gallery-thumbs { display: flex; gap: .45rem; }
.gallery-thumbs--bottom { flex-wrap: wrap; }
.gallery-thumb { background: var(--p-surface-100); border: 1px solid var(--p-content-border-color); border-radius: 4px; cursor: pointer; overflow: hidden; padding: 0; transition: border-color var(--p-transition-duration, .2s); }
.gallery-thumb:hover { border-color: var(--p-primary-color); }
.gallery-thumb img { display: block; height: 56px; object-fit: cover; width: 84px; }
.gallery-thumbs--slider { overflow-x: auto; padding-bottom: .2rem; }
.gallery-thumbs--slider .gallery-thumb { flex: 0 0 auto; }
.gallery-thumbs--slider .gallery-thumb img { height: 96px; width: 150px; }
.gallery-empty { align-items: center; background: var(--p-surface-50); border: 1px dashed var(--p-content-border-color); border-radius: var(--p-border-radius-md, .5rem); color: var(--p-text-muted-color); display: flex; flex-direction: column; gap: .45rem; justify-content: center; min-height: 120px; }
.gallery-empty i { color: var(--p-primary-color); font-size: 1.25rem; }
.gallery-add { display: flex; gap: .5rem; margin-top: .6rem; }
.gallery-add input { flex: 1; min-width: 0; }
.gallery-add__button { align-items: center; background: var(--p-primary-color); border: 1px solid var(--p-primary-color); border-radius: var(--p-button-border-radius); color: var(--p-primary-contrast-color); cursor: pointer; display: inline-flex; justify-content: center; min-width: 2.5rem; }
.gallery-settings { display: flex; flex-direction: column; gap: .5rem; min-width: 13rem; }
.gallery-settings__label { color: var(--p-text-muted-color); font-size: .75rem; font-weight: 600; }
</style>
