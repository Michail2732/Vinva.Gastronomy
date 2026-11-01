import { computed, ref } from 'vue'
import type { Ref } from 'vue'
import type { JSONContent } from '@tiptap/core'
import type { Editor } from '@tiptap/vue-3'
import type { RecipeDraftViewModel, RecipeCharacteristicViewModel, RecipeIngredientViewModel, RecipeIngredientsSetViewModel } from '../types/recipeTypes'
import type { IngredientRowViewModel } from '../ingredientListModel'
import { useRecipesStore } from '../stores/recipesStore'

export type RecipeDocumentAggregate = RecipeDraftViewModel & {
    recipeId: string | null
}

function mapCharacteristicNode(attrs: Record<string, unknown>): RecipeCharacteristicViewModel | null {
    const name = String(attrs.characteristicName ?? '').trim()
    if (!name) return null
    const measure = String(attrs.measure ?? '').trim() || null
    const base = { name, measure, state: 'New' as const }
    switch (attrs.characteristicType) {
        case 'Number': {
            const value = Number(attrs.numberValue)
            if (!Number.isFinite(value)) return null
            return { ...base, type: 'Number', value }
        }
        case 'Range': {
            const min = Number(attrs.rangeMin)
            const max = Number(attrs.rangeMax)
            if (!Number.isFinite(min) || !Number.isFinite(max)) return null
            return { ...base, type: 'Range', value: { min, max } }
        }
        case 'Array': {
            const items = Array.isArray(attrs.arrayItems)
                ? attrs.arrayItems.map((item) => String(item).trim()).filter(Boolean)
                : []
            return { ...base, type: 'Array', value: items }
        }
        default:
            return { ...base, type: 'String', value: String(attrs.stringValue ?? '') }
    }
}

export function useRecipeAggregate(options: {
    title: Ref<string>
    initialDocument: JSONContent
    normalize: (document: JSONContent) => JSONContent
}) {
    const recipesStore = useRecipesStore()
    const isSaving = ref(false)
    const saveStatus = ref('Черновик не сохранён')
    const saveStatusIcon = computed(() => saveStatus.value.startsWith('Не удалось') ? 'pi pi-exclamation-circle' : 'pi pi-check-circle')

    const recipeAggregate = ref<RecipeDocumentAggregate>({
        recipeId: null,
        name: options.title.value,
        description: '',
        document: options.normalize(options.initialDocument),
        characteristics: [],
        ingredientsSets: [],
    })

    function synchronizeAggregate(document: JSONContent, editor: Editor) {
        const setNodes = (document.content ?? [])
            .filter((node) => node.type === 'ingredientsSet')
        recipeAggregate.value.document = document
        recipeAggregate.value.description = editor.getText({ blockSeparator: '\n' }).trim()

        recipeAggregate.value.ingredientsSets = setNodes.map<RecipeIngredientsSetViewModel>((node) => {
            const attrs = node.attrs ?? {}
            const rows = (attrs.ingredients as IngredientRowViewModel[] | undefined) ?? []
            const portionsAmount = Number(String(attrs.portionsAmount ?? '').replace(',', '.'))
            return {
                name: String(attrs.title ?? '').trim() || 'Основное',
                portionsQuantities: portionsAmount > 0 && String(attrs.portionsUnit ?? '').trim()
                    ? [{ amount: portionsAmount, unit: String(attrs.portionsUnit).trim() }]
                    : [],
                portionsCalculateType: attrs.portionsCalculateType === 'MultiSets' ? 'MultiSets' : 'Multiplication',
                ingredients: rows
                    .filter((row) => row.name.trim())
                    .map<RecipeIngredientViewModel>((row) => ({
                        name: row.name.trim(),
                        quantities: row.quantities
                            .map((quantity) => {
                                const amount = Number(quantity.amount.replace(',', '.'))
                                return amount > 0 && quantity.unit.trim()
                                    ? { amount, unit: quantity.unit.trim() }
                                    : null
                            })
                            .filter((quantity): quantity is { amount: number; unit: string } => quantity !== null),
                    })),
            }
        })
        recipeAggregate.value.characteristics = (document.content ?? [])
            .filter((node) => node.type === 'recipeCharacteristic')
            .map((node) => mapCharacteristicNode(node.attrs ?? {}))
            .filter((characteristic): characteristic is RecipeCharacteristicViewModel => characteristic !== null)
        saveStatus.value = 'Есть несохранённые изменения'
    }

    async function saveDraft(editor: Editor) {
        synchronizeAggregate(editor.getJSON(), editor)
        recipeAggregate.value.name = options.title.value.trim()
        recipeAggregate.value.description ||= recipeAggregate.value.name

        if (!recipeAggregate.value.name) {
            saveStatus.value = 'Не удалось сохранить: укажите название'
            return
        }

        localStorage.setItem('recipe-draft', JSON.stringify({ title: recipeAggregate.value.name, document: recipeAggregate.value.document }))
        isSaving.value = true
        saveStatus.value = 'Сохраняем рецепт...'

        try {
            if (recipeAggregate.value.recipeId) {
                const result = await recipesStore.updateRecipe(recipeAggregate.value.recipeId, recipeAggregate.value)
                if (!result.isSuccess) throw new Error(result.error)
            } else {
                const result = await recipesStore.createRecipe(recipeAggregate.value)
                if (!result.isSuccess) throw new Error(result.error)
                const recipeId = result.data.id
                if (!recipeId) throw new Error('Сервер не вернул идентификатор рецепта')
                recipeAggregate.value.recipeId = recipeId
            }
            saveStatus.value = 'Рецепт сохранён'
        } catch (error) {
            saveStatus.value = `Не удалось сохранить: ${error instanceof Error ? error.message : 'ошибка сервера'}`
        } finally {
            isSaving.value = false
        }
    }

    return {
        recipeAggregate,
        isSaving,
        saveStatus,
        saveStatusIcon,
        synchronizeAggregate,
        saveDraft,
    }
}
