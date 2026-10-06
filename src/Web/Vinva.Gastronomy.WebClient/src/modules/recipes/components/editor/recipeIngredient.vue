<template>
	<NodeViewWrapper class="recipe-block recipe-ingredients-block">
		<RecipeNodeToolbar :editable="editor.isEditable" :delete-node="deleteNode" delete-label="Удалить список ингредиентов" />
		<div class="recipe-block__label" :style="{ color: node.attrs.titleColor || undefined }">
			<i class="pi pi-shopping-bag" />
			<input v-if="editor.isEditable" class="recipe-block__title" :value="node.attrs.title" aria-label="Заголовок списка ингредиентов" @input="updateTitle" />
			<span v-else>{{ node.attrs.title }}</span>
		</div>
		<div v-if="editor.isEditable" class="ingredient-rows">
			<div v-for="(ingredient, index) in ingredients" :key="ingredient.id" class="ingredient-row">
				<button class="ingredient-row__remove" type="button" :aria-label="`Удалить ингредиент ${ingredient.name || index + 1}`" title="Удалить ингредиент" @click="removeIngredient(ingredient.id)">
					<i class="pi pi-trash" />
				</button>
				<input class="ingredient-row__input ingredient-row__name" :data-ingredient-id="ingredient.id" :style="ingredientTextStyle(ingredient)" :value="ingredient.name" placeholder="Название ингредиента" :aria-label="`Название ингредиента ${index + 1}`" @input="updateIngredientName(ingredient.id, ($event.target as HTMLInputElement).value)" />
				<div class="ingredient-row__quantities">
					<div v-for="(quantity, quantityIndex) in ingredient.quantities" :key="quantity.id" class="ingredient-quantity-row">
						<input class="ingredient-row__input ingredient-row__amount" :value="quantity.amount" placeholder="Значение" :aria-label="`Значение ингредиента ${index + 1}, строка ${quantityIndex + 1}`" @input="updateQuantity(ingredient.id, quantity.id, 'amount', ($event.target as HTMLInputElement).value)" />
						<input class="ingredient-row__input ingredient-row__unit" :value="quantity.unit" placeholder="Единица измерения" :aria-label="`Единица измерения ингредиента ${index + 1}, строка ${quantityIndex + 1}`" @input="updateQuantity(ingredient.id, quantity.id, 'unit', ($event.target as HTMLInputElement).value)" />
						<button v-if="ingredient.quantities.length > 1" class="ingredient-quantity-remove" type="button" :aria-label="`Удалить количество ${quantityIndex + 1} ингредиента ${index + 1}`" @click="removeQuantity(ingredient.id, quantity.id)">
							<i class="pi pi-times" />
						</button>
					</div>
					<button class="ingredient-quantity-add" type="button" title="Добавить значение и единицу измерения" :aria-label="`Добавить значение для ингредиента ${index + 1}`" @click="addQuantity(ingredient.id)">
						<i class="pi pi-plus" />
					</button>
				</div>
				<textarea class="ingredient-row__input ingredient-row__note" :value="ingredient.note" placeholder="Примечание к ингредиенту" :aria-label="`Примечание к ингредиенту ${index + 1}`" @input="updateIngredientNote(ingredient.id, ($event.target as HTMLTextAreaElement).value)" />
			</div>
			<button class="ingredient-add" type="button" title="Добавить ингредиент" @click="addIngredient">
				<i class="pi pi-plus" />
				<span>Добавить ингредиент</span>
			</button>
		</div>
		<div v-else class="ingredient-readonly">
			<section v-for="group in ingredientGroups" :key="group.part || 'default'">
				<h3 v-if="group.part">{{ group.part }}</h3>
				<ul>
					<li v-for="ingredient in group.ingredients" :key="ingredient.id">
						<div class="ingredient-readonly__main">
							<span>{{ ingredient.name }}</span>
							<span v-if="ingredient.quantities.length" class="ingredient-readonly__quantity">
								{{ ingredient.quantities.map((quantity) => [quantity.amount, quantity.unit].filter(Boolean).join(' ')).filter(Boolean).join(', ') }}
							</span>
						</div>
						<p v-if="ingredient.note" class="ingredient-readonly__note"><i class="pi pi-quote-left" /><strong>Примечание</strong><span>{{ ingredient.note }}</span></p>
					</li>
				</ul>
			</section>
		</div>
	</NodeViewWrapper>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { NodeViewWrapper } from '@tiptap/vue-3'
import type { IngredientRowViewModel, IngredientTextStyle } from '../../ingredientListModel'
import { createIngredientQuantityRow, createIngredientRow } from '../../ingredientListModel'
import RecipeNodeToolbar from './recipeNodeToolbar.vue'

const props = defineProps<{
	editor: { isEditable: boolean }
	node: { attrs: { title: string; titleColor?: string | null; ingredients?: IngredientRowViewModel[] } }
	updateAttributes: (attributes: Record<string, unknown>) => void
	deleteNode: () => void
}>()

const ingredients = computed(() => props.node.attrs.ingredients ?? [])
const ingredientGroups = computed(() => {
	const groups = new Map<string, IngredientRowViewModel[]>()
	for (const ingredient of ingredients.value) {
		const part = ingredient.part?.trim() ?? ''
		groups.set(part, [...(groups.get(part) ?? []), ingredient])
	}
	return Array.from(groups, ([part, values]) => ({ part, ingredients: values }))
})

function addIngredient() {
	props.updateAttributes({ ingredients: [...ingredients.value, createIngredientRow()] })
}

function updateTitle(event: Event) {
	props.updateAttributes({ title: (event.target as HTMLInputElement).value })
}

function updateIngredientName(id: string, value: string) {
	props.updateAttributes({ ingredients: ingredients.value.map((ingredient) => ingredient.id === id ? { ...ingredient, name: value } : ingredient) })
}

function updateIngredientNote(id: string, value: string) {
	props.updateAttributes({ ingredients: ingredients.value.map((ingredient) => ingredient.id === id ? { ...ingredient, note: value } : ingredient) })
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
.recipe-ingredients-block { margin: 1.5rem 0; position: relative; }
.recipe-block__label { color: var(--p-primary-700); font-size: .67rem; font-weight: 700; letter-spacing: .08em; margin-bottom: .55rem; text-transform: uppercase; }
.recipe-block__label i { margin-right: .35rem; }
.recipe-block__title { background: transparent; border: 0; color: inherit; font: inherit; min-width: 0; outline: none; text-transform: inherit; }
.recipe-block__title:focus { box-shadow: 0 1px 0 var(--p-primary-color); }
.ingredient-rows { display: grid; gap: .65rem; }
.ingredient-row { align-items: start; display: grid; gap: .4rem; grid-template-columns: 1.6rem minmax(0, 1fr) minmax(0, 1.2fr); }
.ingredient-row__remove, .ingredient-add { align-items: center; background: transparent; border: 1px solid transparent; border-radius: 3px; color: var(--p-text-muted-color); cursor: pointer; display: inline-flex; justify-content: center; min-height: 2.25rem; }
.ingredient-row__remove:hover { background: var(--p-red-50); border-color: var(--p-red-100); color: var(--p-red-600); }
.ingredient-row__input { background: var(--p-surface-0); border: 1px solid var(--p-content-border-color); border-radius: 3px; color: var(--p-text-color); font-size: .82rem; min-height: 2rem; min-width: 0; padding: .35rem .45rem; width: 100%; }
.ingredient-row__input:focus { border-color: var(--p-primary-color); outline: 2px solid color-mix(in srgb, var(--p-primary-color) 18%, transparent); }
.ingredient-row__input::placeholder { color: var(--p-form-field-placeholder-color); }
.ingredient-row__note { grid-column: 2 / -1; min-height: 2.5rem; resize: vertical; }
.ingredient-row__quantities { display: grid; gap: .35rem; }
.ingredient-quantity-row { display: grid; gap: .25rem; grid-template-columns: minmax(0, .6fr) minmax(0, 1fr) 1.25rem; }
.ingredient-quantity-remove { align-items: center; background: transparent; border: 0; color: var(--p-text-muted-color); cursor: pointer; display: inline-flex; justify-content: center; }
.ingredient-quantity-remove:hover { color: var(--p-red-600); }
.ingredient-quantity-add { align-items: center; background: transparent; border: 0; color: var(--p-primary-color); cursor: pointer; display: inline-flex; font-size: .7rem; justify-content: center; justify-self: start; min-height: 1.25rem; padding: .1rem .35rem; }
.ingredient-quantity-add:hover { background: var(--p-primary-50); border-radius: 3px; }
.ingredient-add { color: var(--p-primary-color); gap: .45rem; justify-content: flex-start; padding: .35rem .5rem; width: fit-content; }
.ingredient-add:hover { background: var(--p-primary-50); }
.ingredient-readonly section + section { border-top: 1px solid var(--p-content-border-color); margin-top: .8rem; padding-top: .65rem; }
.ingredient-readonly h3 { color: var(--p-primary-700); font-size: .78rem; margin: 0 0 .35rem; }
.ingredient-readonly ul { display: grid; gap: .3rem; list-style: none; margin: 0; padding: 0; }
.ingredient-readonly li { display: grid; gap: .35rem; }
.ingredient-readonly__main { display: flex; flex-wrap: wrap; gap: .3rem .6rem; justify-content: space-between; }
.ingredient-readonly__quantity { color: var(--p-text-muted-color); }
.ingredient-readonly__note { align-items: baseline; border-left: 2px solid var(--p-primary-200); color: var(--p-text-muted-color); display: flex; flex-wrap: wrap; font-size: .76rem; gap: .25rem .4rem; margin: 0; padding-left: .55rem; }
.ingredient-readonly__note i { color: var(--p-primary-color); }
.ingredient-readonly__note strong { color: var(--p-primary-700); font-weight: 600; }
@media (max-width: 850px) { .recipe-ingredients-block { margin: 1rem 0; } .ingredient-row { gap: .35rem; grid-template-columns: 1.5rem minmax(0, 1fr) minmax(0, 1.2fr); } .ingredient-row__quantities, .ingredient-row__note { grid-column: 2 / -1; } }
</style>
