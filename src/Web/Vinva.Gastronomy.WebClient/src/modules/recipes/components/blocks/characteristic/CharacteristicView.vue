<template>
    <NodeViewWrapper class="recipe-block recipe-characteristic-block" :style="spacingStyle">
        <BlockDragHandle v-if="isEditable" :editor="editor" />
        <RecipeNodeToolbar :editable="isEditable" :delete-node="deleteNode" delete-label="Удалить характеристику">
            <RecipeBlockSpacing :attrs="node.attrs" :update-attributes="updateAttributes" />
        </RecipeNodeToolbar>
        <div v-if="isEditable" class="characteristic-form">
            <div class="characteristic-form__row">
                <input
                    class="characteristic-input characteristic-form__name"
                    :value="node.attrs.characteristicName"
                    placeholder="Название характеристики"
                    aria-label="Название характеристики"
                    @input="updateFromInput('characteristicName', $event)"
                    @focus="focusBlock"
                />
                <Select
                    :model-value="node.attrs.characteristicType"
                    :options="typeOptions"
                    option-label="label"
                    option-value="value"
                    aria-label="Тип характеристики"
                    class="characteristic-form__type"
                    @mousedown.prevent
                    @update:model-value="updateAttribute('characteristicType', $event)"
                />
                <input
                    class="characteristic-input characteristic-form__measure"
                    :value="node.attrs.measure"
                    placeholder="Ед. изм."
                    aria-label="Единица измерения"
                    @input="updateFromInput('measure', $event)"
                    @focus="focusBlock"
                />
            </div>
            <div class="characteristic-form__value">
                <input
                    v-if="node.attrs.characteristicType === 'String'"
                    class="characteristic-input"
                    :value="node.attrs.stringValue"
                    placeholder="Значение"
                    aria-label="Значение характеристики"
                    @input="updateFromInput('stringValue', $event)"
                    @focus="focusBlock"
                />
                <InputNumber
                    v-else-if="node.attrs.characteristicType === 'Number'"
                    :model-value="node.attrs.numberValue"
                    :use-grouping="false"
                    placeholder="Значение"
                    aria-label="Числовое значение характеристики"
                    @update:model-value="updateAttribute('numberValue', $event)"
                />
                <div v-else-if="node.attrs.characteristicType === 'Range'" class="characteristic-form__range">
                    <InputNumber
                        class="characteristic-form__range-input"
                        :model-value="node.attrs.rangeMin"
                        :use-grouping="false"
                        placeholder="Мин"
                        aria-label="Минимальное значение"
                        @update:model-value="updateAttribute('rangeMin', $event)"
                    />
                    <span class="characteristic-form__range-separator">—</span>
                    <InputNumber
                        class="characteristic-form__range-input"
                        :model-value="node.attrs.rangeMax"
                        :use-grouping="false"
                        placeholder="Макс"
                        aria-label="Максимальное значение"
                        @update:model-value="updateAttribute('rangeMax', $event)"
                    />
                </div>
                <div v-else class="characteristic-form__array">
                    <div v-for="(item, index) in arrayItems" :key="index" class="characteristic-form__array-row">
                        <input
                            class="characteristic-input"
                            :value="item"
                            placeholder="Элемент"
                            :aria-label="`Элемент ${index + 1}`"
                            @input="updateArrayItem(index, ($event.target as HTMLInputElement).value)"
                            @focus="focusBlock"
                        />
                        <button class="characteristic-array-remove" type="button" :aria-label="`Удалить элемент ${index + 1}`" @click="removeArrayItem(index)">
                            <i class="pi pi-times" />
                        </button>
                    </div>
                    <button class="characteristic-array-add" type="button" title="Добавить элемент" @click="addArrayItem">
                        <i class="pi pi-plus" />
                        <span>Добавить элемент</span>
                    </button>
                </div>
            </div>
        </div>
        <div v-else class="characteristic-readonly">
            <span class="characteristic-readonly__name">{{ node.attrs.characteristicName || 'Характеристика' }}:</span>
            <span v-if="valueText" class="characteristic-readonly__value">{{ valueText }}{{ node.attrs.measure ? ` ${node.attrs.measure}` : '' }}</span>
        </div>
    </NodeViewWrapper>
</template>

<script setup lang="ts">
import { computed, inject, ref } from 'vue'
import type { Ref } from 'vue'
import type { Editor } from '@tiptap/core'
import { NodeViewWrapper } from '@tiptap/vue-3'
import { NodeSelection } from '@tiptap/pm/state'
import Select from 'primevue/select'
import InputNumber from 'primevue/inputnumber'
import RecipeNodeToolbar from '../../editor/recipeNodeToolbar.vue'
import RecipeBlockSpacing from '../../editor/recipeBlockSpacing.vue'
import { blockMarginStyle } from '../../shared/margins'
import BlockDragHandle from '../drag/BlockDragHandle.vue'

const typeOptions = [
    { label: 'Строка', value: 'String' },
    { label: 'Число', value: 'Number' },
    { label: 'Диапазон', value: 'Range' },
    { label: 'Список', value: 'Array' },
]

const props = defineProps<{
    editor: Editor
    node: { attrs: {
        characteristicName?: string
        characteristicType?: string
        measure?: string
        stringValue?: string
        numberValue?: number | null
        rangeMin?: number | null
        rangeMax?: number | null
        arrayItems?: string[]
    } }
    getPos: () => number | undefined
    updateAttributes: (attributes: Record<string, unknown>) => void
    deleteNode: () => void
}>()

const isEditable = inject<Ref<boolean>>('editorEditable', ref(true))
const spacingStyle = computed(() => blockMarginStyle(props.node.attrs as unknown as Record<string, unknown>))
const arrayItems = computed(() => props.node.attrs.arrayItems ?? [])

const valueText = computed(() => {
    const attrs = props.node.attrs
    switch (attrs.characteristicType) {
        case 'Number':
            return attrs.numberValue !== null && attrs.numberValue !== undefined ? String(attrs.numberValue) : ''
        case 'Range':
            return [attrs.rangeMin, attrs.rangeMax].filter((v) => v !== null && v !== undefined).join(' – ')
        case 'Array':
            return (attrs.arrayItems ?? []).filter(Boolean).join(', ')
        default:
            return attrs.stringValue ?? ''
    }
})

function updateAttribute(field: string, value: unknown) {
    props.updateAttributes({ [field]: value })
}

function updateFromInput(field: string, event: Event) {
    updateAttribute(field, (event.target as HTMLInputElement).value)
}

function focusBlock() {
    const position = props.getPos()
    if (position === undefined) return
    const { state, dispatch } = props.editor.view
    const selection = NodeSelection.create(state.doc, position)
    dispatch(state.tr.setSelection(selection))
}

function updateArrayItem(index: number, value: string) {
    const items = [...arrayItems.value]
    items[index] = value
    updateAttribute('arrayItems', items)
}

function addArrayItem() {
    updateAttribute('arrayItems', [...arrayItems.value, ''])
}

function removeArrayItem(index: number) {
    updateAttribute('arrayItems', arrayItems.value.filter((_, i) => i !== index))
}
</script>

<style scoped lang="scss">
.recipe-characteristic-block { margin: 1rem 0; position: relative; }
.characteristic-form { display: grid; gap: .5rem; font-size: .82rem;}
.characteristic-form__row { display: grid; gap: .4rem; grid-template-columns: minmax(0, 1.5fr) minmax(0, 1fr) minmax(0, .6fr); }
.characteristic-form__name { font-weight: 600; }
.characteristic-form__type { min-width: 0; }
.characteristic-form__type :deep(.p-select-label) { padding: .35rem .45rem; }
.characteristic-form__value { display: grid; gap: .35rem; }
.characteristic-form__range { align-items: center; display: flex; gap: .5rem; }
.characteristic-form__range-separator { color: var(--p-text-muted-color); margin: 0 .2rem; }
.characteristic-form__range-input { width: auto; }
.characteristic-form__array { display: grid; gap: .35rem; }
.characteristic-form__array-row { display: grid; gap: .25rem; grid-template-columns: minmax(0, 1fr) 1.5rem; }
.characteristic-input { background: var(--p-surface-0); border: 1px solid var(--p-content-border-color); border-radius: 3px; color: var(--p-text-color); font-size: 1rem; min-height: 2rem; min-width: 0; padding: .35rem .45rem; width: 100%; }
.characteristic-input:focus { border-color: var(--p-primary-color); outline: 2px solid color-mix(in srgb, var(--p-primary-color) 18%, transparent); }
.characteristic-input::placeholder { color: var(--p-form-field-placeholder-color); }
.characteristic-array-remove { align-items: center; background: transparent; border: 0; color: var(--p-text-muted-color); cursor: pointer; display: inline-flex; justify-content: center; }
.characteristic-array-remove:hover { color: var(--p-red-600); }
.characteristic-array-add { align-items: center; background: transparent; border: 0; color: var(--p-primary-color); cursor: pointer; display: inline-flex; font-size: .75rem; gap: .35rem; justify-content: flex-start; min-height: 1.5rem; padding: .15rem .35rem; width: fit-content; }
.characteristic-array-add:hover { background: var(--p-primary-50); border-radius: 3px; }
.characteristic-readonly { display: flex; flex-wrap: wrap; gap: .3rem .5rem; }
.characteristic-readonly__name { font-weight: 600; }
.characteristic-readonly__value { color: var(--p-text-muted-color); }
@media (max-width: 850px) {
    .characteristic-form__row { grid-template-columns: 1fr; }
    .characteristic-form__measure { max-width: none; }
}
</style>
