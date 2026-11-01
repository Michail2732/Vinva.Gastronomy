<template>
	<NodeViewWrapper class="recipe-block recipe-ingredients-block" :style="[blockStyle, spacingStyle]">
		<BlockDragHandle v-if="isEditable" :editor="editor" />
		<RecipeNodeToolbar :editable="isEditable" :delete-node="deleteNode" delete-label="Удалить набор ингредиентов">
			<Button :icon="node.attrs.icon || defaultIcon" text severity="secondary" aria-label="Значок блока" title="Значок блока" @mousedown.prevent @click="toggleIconPopover" />
			<Button icon="pi pi-sliders-h" text severity="secondary" aria-label="Настройки рамки" title="Настройки рамки" @mousedown.prevent @click="toggleBorderPopover" />
			<RecipeBlockSpacing :attrs="node.attrs" :update-attributes="updateAttributes" />
		</RecipeNodeToolbar>
		<Popover ref="iconPopover">
			<div class="ingredients-set-icon-grid" aria-label="Доступные значки">
				<Button
					v-for="icon in boxIcons"
					:key="icon.value"
					:icon="icon.value"
					text
					severity="secondary"
					:aria-label="icon.label"
					:title="icon.label"
					:class="{ active: node.attrs.icon === icon.value }"
					@click="selectIcon(icon.value)"
				/>
			</div>
		</Popover>
		<Popover ref="borderPopover">
			<div class="ingredients-set-border-settings" aria-label="Настройки границ">
				<label class="ingredients-set-border-settings__color"><span>Цвет границы</span><input type="color" aria-label="Цвет границы" :value="node.attrs.borderColor || defaultBorderColor" @input="updateFromInput('borderColor', $event)" /></label>
				<label class="ingredients-set-border-settings__color"><span>Цвет фона</span><input type="color" aria-label="Цвет фона блока" :value="node.attrs.backgroundColor || defaultBackgroundColor" @input="updateFromInput('backgroundColor', $event)" /></label>
				<Button
					v-if="node.attrs.backgroundColor"
					class="ingredients-set-border-settings__reset"
					icon="pi pi-undo"
					text
					severity="secondary"
					label="Сбросить фон"
					size="small"
					:aria-label="'Сбросить цвет фона'"
					@mousedown.prevent
					@click="resetBackground"
				/>
				<label v-for="edge in borderEdges" :key="edge.field" class="ingredients-set-border-settings__number">
					<span>{{ edge.label }}</span>
					<InputNumber
						:model-value="node.attrs[edge.field] ?? edge.defaultValue"
						:min="0"
						:max="20"
						:use-grouping="false"
						suffix=" px"
						:aria-label="`Ширина границы ${edge.label.toLocaleLowerCase()}`"
						@update:model-value="updateAttribute(edge.field, $event ?? 0)"
					/>
				</label>
				<label class="ingredients-set-border-settings__number">
					<span>Скругление</span>
					<InputNumber
						:model-value="node.attrs.borderRadius ?? 6"
						:min="0"
						:max="32"
						:use-grouping="false"
						suffix=" px"
						aria-label="Скругление границ"
						@update:model-value="updateAttribute('borderRadius', $event ?? 0)"
					/>
				</label>
			</div>
		</Popover>
		<div class="group-box__heading" :style="{ color: node.attrs.titleColor || undefined }">
			<i :class="node.attrs.icon || defaultIcon" aria-hidden="true" />
			<input
				v-if="isEditable"
				class="group-box__title"
				:value="node.attrs.title || 'Ингредиенты'"
				placeholder="Название набора"
				aria-label="Название набора ингредиентов"
				@input="updateFromInput('title', $event)"
				@focus="focusBlock"
			/>
			<strong v-else>{{ node.attrs.title || 'Ингредиенты' }}</strong>
		</div>
		<div v-if="isEditable" class="ingredient-portions">
			<label class="ingredient-portions__label">Количество порций</label>
			<div class="ingredient-portions__fields">
				<input
					class="ingredient-row__input ingredient-row__amount"
					:value="node.attrs.portionsAmount"
					placeholder="Значение"
					aria-label="Количество порций, значение"
					@input="updateFromInput('portionsAmount', $event)"
					@focus="focusBlock"
				/>
				<input
					class="ingredient-row__input ingredient-row__unit"
					:value="node.attrs.portionsUnit"
					placeholder="Единица измерения"
					aria-label="Количество порций, единица измерения"
					@input="updateFromInput('portionsUnit', $event)"
					@focus="focusBlock"
				/>
			</div>
		</div>
		<p v-else-if="portionsText" class="ingredient-portions ingredient-portions--readonly">{{ portionsText }}</p>
		<div v-if="isEditable" class="ingredient-rows">
			<div v-for="(ingredient, index) in ingredients" :key="ingredient.id" class="ingredient-row">
				<button class="ingredient-row__remove" type="button" :aria-label="`Удалить ингредиент ${ingredient.name || index + 1}`" title="Удалить ингредиент" @click="removeIngredient(ingredient.id)">
					<i class="pi pi-trash" />
				</button>
				<input class="ingredient-row__input ingredient-row__name" :data-ingredient-id="ingredient.id" :style="ingredientTextStyle(ingredient)" :value="ingredient.name" placeholder="Название ингредиента" :aria-label="`Название ингредиента ${index + 1}`" @input="updateIngredientName(ingredient.id, ($event.target as HTMLInputElement).value)" @focus="focusBlock" />
				<div class="ingredient-row__quantities">
					<div v-for="(quantity, quantityIndex) in ingredient.quantities" :key="quantity.id" class="ingredient-quantity-row">
					<input class="ingredient-row__input ingredient-row__amount" :value="quantity.amount" placeholder="Значение" :aria-label="`Значение ингредиента ${index + 1}, строка ${quantityIndex + 1}`" @input="updateQuantity(ingredient.id, quantity.id, 'amount', ($event.target as HTMLInputElement).value)" @focus="focusBlock" />
					<input class="ingredient-row__input ingredient-row__unit" :value="quantity.unit" placeholder="Единица измерения" :aria-label="`Единица измерения ингредиента ${index + 1}, строка ${quantityIndex + 1}`" @input="updateQuantity(ingredient.id, quantity.id, 'unit', ($event.target as HTMLInputElement).value)" @focus="focusBlock" />
						<button v-if="ingredient.quantities.length > 1" class="ingredient-quantity-remove" type="button" :aria-label="`Удалить количество ${quantityIndex + 1} ингредиента ${index + 1}`" @click="removeQuantity(ingredient.id, quantity.id)">
							<i class="pi pi-times" />
						</button>
					</div>
					<button class="ingredient-quantity-add" type="button" title="Добавить значение и единицу измерения" :aria-label="`Добавить значение для ингредиента ${index + 1}`" @click="addQuantity(ingredient.id)">
						<i class="pi pi-plus" />
					</button>
				</div>

			</div>
			<button class="ingredient-add" type="button" title="Добавить ингредиент" @click="addIngredient">
				<i class="pi pi-plus" />
				<span>Добавить ингредиент</span>
			</button>
		</div>
		<div v-else class="ingredient-readonly">
			<ul>
				<li v-for="ingredient in ingredients" :key="ingredient.id">
					<div class="ingredient-readonly__main">
						<span>{{ ingredient.name }}</span>
						<span v-if="ingredient.quantities.length" class="ingredient-readonly__quantity">
							{{ ingredient.quantities.map((quantity) => [quantity.amount, quantity.unit].filter(Boolean).join(' ')).filter(Boolean).join(', ') }}
						</span>
					</div>

				</li>
			</ul>
		</div>
	</NodeViewWrapper>
</template>

<script setup lang="ts">
import { computed, inject, ref } from 'vue'
import type { Ref } from 'vue'
const isEditable = inject<Ref<boolean>>('editorEditable', ref(true))
import type { Editor } from '@tiptap/core'
import { NodeViewWrapper } from '@tiptap/vue-3'
import { NodeSelection } from '@tiptap/pm/state'
import Popover from 'primevue/popover'
import type { IngredientRowViewModel, IngredientTextStyle } from '../../../ingredientListModel'
import { createIngredientQuantityRow, createIngredientRow } from '../../../ingredientListModel'
import RecipeNodeToolbar from '../../editor/recipeNodeToolbar.vue'
import RecipeBlockSpacing from '../../editor/recipeBlockSpacing.vue'
import { blockMarginStyle } from '../../shared/margins'
import BlockDragHandle from '../drag/BlockDragHandle.vue'

const defaultIcon = 'pi pi-shopping-bag'
const defaultBorderColor = '#b7c8bb'
const defaultBackgroundColor = '#ffffff'
const boxIcons = [
	{ label: 'Ингредиенты', value: 'pi pi-shopping-bag' },
	{ label: 'Информация', value: 'pi pi-info-circle' },
	{ label: 'Комментарий', value: 'pi pi-comment' },
	{ label: 'Заметка', value: 'pi pi-bookmark' },
	{ label: 'Совет', value: 'pi pi-lightbulb' },
	{ label: 'Внимание', value: 'pi pi-exclamation-triangle' },
	{ label: 'Список задач', value: 'pi pi-list-check' },
	{ label: 'Рецепт', value: 'pi pi-star' },
]
const borderEdges = [
	{ field: 'borderTopWidth', label: 'Верх', defaultValue: 1 },
	{ field: 'borderRightWidth', label: 'Правая', defaultValue: 1 },
	{ field: 'borderBottomWidth', label: 'Низ', defaultValue: 1 },
	{ field: 'borderLeftWidth', label: 'Левая', defaultValue: 3 },
] as const

const props = defineProps<{
	editor: Editor
	node: { attrs: {
		title: string
		titleColor?: string | null
		icon?: string | null
		portionsAmount?: string
		portionsUnit?: string
		portionsCalculateType?: string
		borderColor?: string | null
		backgroundColor?: string | null
		borderTopWidth?: number | null
		borderRightWidth?: number | null
		borderBottomWidth?: number | null
		borderLeftWidth?: number | null
		borderRadius?: number | null
		ingredients?: IngredientRowViewModel[]
	} }
	getPos: () => number | undefined
	updateAttributes: (attributes: Record<string, unknown>) => void
	deleteNode: () => void
}>()

const blockStyle = computed(() => ({
	backgroundColor: String(props.node.attrs.backgroundColor || 'var(--p-surface-0)'),
	borderColor: String(props.node.attrs.borderColor || defaultBorderColor),
	borderStyle: 'solid',
	borderWidth: borderEdges.map(({ field, defaultValue }) => `${Math.max(0, Number(props.node.attrs[field] ?? defaultValue))}px`).join(' '),
	borderRadius: `${Math.max(0, Number(props.node.attrs.borderRadius ?? 6))}px`,
}))

const spacingStyle = computed(() => blockMarginStyle(props.node.attrs as unknown as Record<string, unknown>))
const ingredients = computed(() => props.node.attrs.ingredients ?? [])
const portionsText = computed(() => {
	const amount = props.node.attrs.portionsAmount?.trim()
	const unit = props.node.attrs.portionsUnit?.trim()
	if (!amount && !unit) return ''
	return `Количество порций: ${[amount, unit].filter(Boolean).join(' ')}`
})

function updateFromInput(field: string, event: Event) {
	props.updateAttributes({ [field]: (event.target as HTMLInputElement).value })
}

const iconPopover = ref<{ toggle: (event: Event) => void; hide: () => void } | null>(null)
const borderPopover = ref<{ toggle: (event: Event) => void } | null>(null)

function toggleIconPopover(event: MouseEvent) {
	iconPopover.value?.toggle(event)
}

function toggleBorderPopover(event: MouseEvent) {
	borderPopover.value?.toggle(event)
}

function selectIcon(value: string) {
	updateAttribute('icon', value)
	iconPopover.value?.hide()
}

function updateAttribute(field: string, value: string | number | null) {
	props.updateAttributes({ [field]: value })
}

function resetBackground() {
	updateAttribute('backgroundColor', null)
}

function addIngredient() {
	props.updateAttributes({ ingredients: [...ingredients.value, createIngredientRow()] })
}

function focusBlock() {
	const position = props.getPos()
	if (position === undefined) return
	const { state, dispatch } = props.editor.view
	const selection = NodeSelection.create(state.doc, position)
	dispatch(state.tr.setSelection(selection))
}

function updateIngredientName(id: string, value: string) {
	props.updateAttributes({ ingredients: ingredients.value.map((ingredient) => ingredient.id === id ? { ...ingredient, name: value } : ingredient) })
}

function ingredientTextStyle(ingredient: IngredientRowViewModel) {
	const style: IngredientTextStyle = ingredient.textStyle ?? {}
	const decorations = [style.underline ? 'underline' : '', style.strike ? 'line-through' : ''].filter(Boolean).join(' ')
	return {
		fontWeight: style.bold ? '700' : undefined,
		fontStyle: style.italic ? 'italic' : undefined,
		textDecoration: decorations || undefined,
		fontSize: style.fontSize ? `${style.fontSize}px` : undefined,
		fontFamily: style.fontFamily || undefined,
		color: style.color || undefined,
		textAlign: style.textAlign || undefined,
	}
}

function updateQuantity(id: string, quantityId: string, field: 'amount' | 'unit', value: string) {
	props.updateAttributes({
		ingredients: ingredients.value.map((ingredient) => ingredient.id === id
			? { ...ingredient, quantities: ingredient.quantities.map((quantity) => quantity.id === quantityId ? { ...quantity, [field]: value } : quantity) }
			: ingredient),
	})
}

function addQuantity(id: string) {
	props.updateAttributes({ ingredients: ingredients.value.map((ingredient) => ingredient.id === id ? { ...ingredient, quantities: [...ingredient.quantities, createIngredientQuantityRow()] } : ingredient) })
}

function removeQuantity(id: string, quantityId: string) {
	props.updateAttributes({ ingredients: ingredients.value.map((ingredient) => ingredient.id === id ? { ...ingredient, quantities: ingredient.quantities.filter((quantity) => quantity.id !== quantityId) } : ingredient) })
}

function removeIngredient(id: string) {
	props.updateAttributes({ ingredients: ingredients.value.filter((ingredient) => ingredient.id !== id) })
}
</script>

<style scoped lang="scss">
.recipe-ingredients-block { margin: 1.5rem 0; padding: .75rem 1rem; position: relative; }
.ingredients-set-icon-grid { display: grid; gap: .2rem; grid-template-columns: repeat(4, 2.25rem); }
.ingredients-set-icon-grid :deep(.p-button) { height: 2.25rem; padding: 0; width: 2.25rem; }
.ingredients-set-icon-grid :deep(.p-button.active) { background: var(--p-primary-50); color: var(--p-primary-700); }
.ingredients-set-border-settings { display: grid; gap: .55rem; min-width: 13rem; }
.ingredients-set-border-settings__color, .ingredients-set-border-settings__number { align-items: center; color: var(--p-text-muted-color); display: flex; font-size: .75rem; gap: .75rem; justify-content: space-between; }
.ingredients-set-border-settings__reset { justify-content: flex-start; width: 100%; }
.ingredients-set-border-settings__color input { border: 1px solid var(--p-content-border-color); border-radius: 3px; height: 1.7rem; padding: .1rem; width: 2.4rem; }
.ingredients-set-border-settings__number :deep(.p-inputnumber) { width: 5rem; }
.ingredients-set-border-settings__number :deep(.p-inputnumber-input) { font-size: .72rem; min-width: 0; padding: .3rem .15rem; text-align: center; }
.group-box__heading { align-items: center; color: var(--p-primary-700); display: flex; font-size: .9rem; font-weight: 700; gap: .45rem; margin-bottom: .55rem; }
.group-box__heading > i { flex: 0 0 auto; }
.group-box__title { background: transparent; border: 0; color: inherit; font: inherit; min-width: 0; outline: none; width: 100%; }
.group-box__title:focus { box-shadow: 0 1px 0 var(--p-primary-color); }
.ingredient-portions { display: grid; gap: .3rem; margin-bottom: .65rem; }
.ingredient-portions__label { color: var(--p-text-muted-color); font-size: .72rem; font-weight: 700; }
.ingredient-portions__fields { display: grid; gap: .25rem; grid-template-columns: minmax(0, .6fr) minmax(0, 1fr); max-width: 22rem; }
.ingredient-portions--readonly { color: var(--p-text-muted-color); display: block; font-size: .85rem; margin: 0 0 .65rem; }
.ingredient-rows { display: grid; gap: .65rem; }
.ingredient-row { align-items: start; display: grid; gap: .4rem; grid-template-columns: 1.6rem minmax(0, 1fr) minmax(0, 1.2fr); }
.ingredient-row__remove, .ingredient-add { align-items: center; background: transparent; border: 1px solid transparent; border-radius: 3px; color: var(--p-text-muted-color); cursor: pointer; display: inline-flex; justify-content: center; min-height: 2.25rem; }
.ingredient-row__remove:hover { background: var(--p-red-50); border-color: var(--p-red-100); color: var(--p-red-600); }
.ingredient-row__input { background: var(--p-surface-0); border: 1px solid var(--p-content-border-color); border-radius: 3px; color: var(--p-text-color); font-size: .82rem; min-height: 2rem; min-width: 0; padding: .35rem .45rem; width: 100%; }
.ingredient-row__input:focus { border-color: var(--p-primary-color); outline: 2px solid color-mix(in srgb, var(--p-primary-color) 18%, transparent); }
.ingredient-row__input::placeholder { color: var(--p-form-field-placeholder-color); }
.ingredient-row__quantities { display: grid; gap: .35rem; }
.ingredient-quantity-row { display: grid; gap: .25rem; grid-template-columns: minmax(0, .6fr) minmax(0, 1fr) 1.25rem; }
.ingredient-quantity-remove { align-items: center; background: transparent; border: 0; color: var(--p-text-muted-color); cursor: pointer; display: inline-flex; justify-content: center; }
.ingredient-quantity-remove:hover { color: var(--p-red-600); }
.ingredient-quantity-add { align-items: center; background: transparent; border: 0; color: var(--p-primary-color); cursor: pointer; display: inline-flex; font-size: .7rem; justify-content: center; justify-self: start; min-height: 1.25rem; padding: .1rem .35rem; }
.ingredient-quantity-add:hover { background: var(--p-primary-50); border-radius: 3px; }
.ingredient-add { color: var(--p-primary-color); gap: .45rem; justify-content: flex-start; padding: .35rem .5rem; width: fit-content; }
.ingredient-add:hover { background: var(--p-primary-50); }
.ingredient-readonly ul { display: grid; gap: .3rem; list-style: none; margin: 0; padding: 0; }
.ingredient-readonly li { display: grid; gap: .35rem; }
.ingredient-readonly__main { display: flex; flex-wrap: wrap; gap: .3rem .6rem; justify-content: space-between; }
.ingredient-readonly__quantity { color: var(--p-text-muted-color); }
@media (max-width: 850px) { .recipe-ingredients-block { margin: 1rem 0; padding: .7rem .75rem; } .ingredient-row { gap: .35rem; grid-template-columns: 1.5rem minmax(0, 1fr) minmax(0, 1.2fr); } .ingredient-row__quantities { grid-column: 2 / -1; } }
</style>
