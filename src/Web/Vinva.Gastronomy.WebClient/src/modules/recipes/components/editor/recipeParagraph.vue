<template>
    <NodeViewWrapper class="recipe-block recipe-paragraph-block" :style="blockStyle">
        <RecipeNodeToolbar :editable="editor.isEditable" :delete-node="deleteNode" delete-label="Удалить блок">
            <Button :icon="node.attrs.icon || defaultIcon" text severity="secondary" aria-label="Выбрать иконку блока" title="Иконка блока" @mousedown.prevent @click="toggleIconPopover" />
            <Button icon="pi pi-sliders-h" text severity="secondary" aria-label="Настроить границу" title="Настроить границу" @mousedown.prevent @click="toggleBorderPopover" />
        </RecipeNodeToolbar>
        <Popover ref="iconPopover">
            <div class="paragraph-icon-grid" aria-label="Доступные иконки">
                <Button
                    v-for="icon in paragraphIcons"
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
            <div class="paragraph-border-settings" aria-label="Настройки границ">
                <label class="paragraph-border-settings__color"><span>Цвет</span><input type="color" aria-label="Цвет границы" :value="node.attrs.borderColor || defaultBorderColor" @input="updateFromInput('borderColor', $event)" /></label>
                <label v-for="edge in borderEdges" :key="edge.field" class="paragraph-border-settings__number">
                    <span>{{ edge.label }}</span>
                    <InputNumber
                        :model-value="node.attrs[edge.field] ?? 1"
                        :min="0"
                        :max="20"
                        :use-grouping="false"
                        suffix=" px"
                        :aria-label="`Толщина границы ${edge.label.toLocaleLowerCase()}`"
                        @update:model-value="updateAttribute(edge.field, $event ?? 0)"
                    />
                </label>
                <label class="paragraph-border-settings__number">
                    <span>Скругление</span>
                    <InputNumber
                        :model-value="node.attrs.borderRadius ?? 6"
                        :min="0"
                        :max="32"
                        :use-grouping="false"
                        suffix=" px"
                        aria-label="Скругление границы"
                        @update:model-value="updateAttribute('borderRadius', $event ?? 0)"
                    />
                </label>
            </div>
        </Popover>
        <div class="recipe-paragraph__heading" :style="{ color: node.attrs.titleColor || undefined }">
            <i :class="node.attrs.icon || defaultIcon" aria-hidden="true" />
            <input
                v-if="editor.isEditable"
                class="recipe-paragraph__title"
                :value="node.attrs.title || 'Параграф'"
                aria-label="Заголовок блока"
                @input="updateFromInput('title', $event)"
            />
            <strong v-else>{{ node.attrs.title || 'Параграф' }}</strong>
        </div>
        <NodeViewContent as="p" class="recipe-paragraph__content" :style="{ textAlign: node.attrs.textAlign || undefined }" />
        <Button v-if="editor.isEditable && node.attrs.kind === 'steps'" class="recipe-steps__add" icon="pi pi-plus" label="Добавить шаг" text severity="secondary" @mousedown.prevent @click="addStep" />
    </NodeViewWrapper>
</template>

<script setup lang="ts">
import { computed, ref } from 'vue'
    import type { Editor } from '@tiptap/core'
import { NodeViewContent, NodeViewWrapper } from '@tiptap/vue-3'
import Popover from 'primevue/popover'
    import { Fragment } from '@tiptap/pm/model'
    import { TextSelection } from '@tiptap/pm/state'
import RecipeNodeToolbar from './recipeNodeToolbar.vue'

const defaultIcon = 'pi pi-info-circle'
const defaultBorderColor = '#b7c8bb'
const paragraphIcons = [
    { label: 'Информация', value: 'pi pi-info-circle' },
    { label: 'Комментарий', value: 'pi pi-comment' },
    { label: 'Заметка', value: 'pi pi-bookmark' },
    { label: 'Совет', value: 'pi pi-lightbulb' },
    { label: 'Внимание', value: 'pi pi-exclamation-triangle' },
    { label: 'Ингредиенты', value: 'pi pi-shopping-bag' },
    { label: 'Шаги', value: 'pi pi-list-check' },
    { label: 'Избранное', value: 'pi pi-star' },
]
const borderEdges = [
    { field: 'borderTopWidth', label: 'Верх' },
    { field: 'borderRightWidth', label: 'Правая сторона' },
    { field: 'borderBottomWidth', label: 'Низ' },
    { field: 'borderLeftWidth', label: 'Левая сторона' },
] as const

const props = defineProps<{
    editor: Editor
    getPos: () => number | undefined
    deleteNode: () => void
    node: { nodeSize: number; attrs: {
        title?: string | null
        titleColor?: string | null
        kind?: string | null
        icon?: string | null
        borderColor?: string | null
        backgroundColor?: string | null
        borderTopWidth?: number | null
        borderRightWidth?: number | null
        borderBottomWidth?: number | null
        borderLeftWidth?: number | null
        borderRadius?: number | null
        textAlign?: string | null
    } }
    updateAttributes: (attributes: Record<string, unknown>) => void
}>()

const blockStyle = computed(() => ({
    backgroundColor: String(props.node.attrs.backgroundColor || 'var(--p-surface-0)'),
    borderColor: String(props.node.attrs.borderColor || defaultBorderColor),
    borderStyle: 'solid',
    borderWidth: borderEdges.map(({ field }) => `${Math.max(0, Number(props.node.attrs[field] ?? 1))}px`).join(' '),
    borderRadius: `${Math.max(0, Number(props.node.attrs.borderRadius ?? 6))}px`,
}))

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

function updateAttribute(field: string, value: string | number) {
    props.updateAttributes({ [field]: value })
}

function updateFromInput(field: string, event: Event) {
    updateAttribute(field, (event.target as HTMLInputElement).value)
}

function addStep() {
    const position = props.getPos()
    const hardBreak = props.editor.schema.nodes.hardBreak
    if (position === undefined || !hardBreak) return
    const insertPosition = position + props.node.nodeSize - 1
    const content = Fragment.fromArray([
        hardBreak.create(),
        props.editor.schema.text('Новый шаг'),
    ])
    const transaction = props.editor.state.tr.insert(insertPosition, content)
    transaction.setSelection(TextSelection.near(transaction.doc.resolve(insertPosition + content.size)))
    props.editor.view.dispatch(transaction.scrollIntoView())
}
</script>

<style scoped lang="scss">
.recipe-paragraph-block { background: var(--p-surface-0); margin: 1.25rem 0; padding: .75rem 1rem; position: relative; }
.paragraph-icon-grid { display: grid; gap: .2rem; grid-template-columns: repeat(4, 2.25rem); }
.paragraph-icon-grid :deep(.p-button) { height: 2.25rem; padding: 0; width: 2.25rem; }
.paragraph-icon-grid :deep(.p-button.active) { background: var(--p-primary-50); color: var(--p-primary-700); }
.paragraph-border-settings { display: grid; gap: .55rem; min-width: 13rem; }
.paragraph-border-settings__color, .paragraph-border-settings__number { align-items: center; color: var(--p-text-muted-color); display: flex; font-size: .75rem; gap: .75rem; justify-content: space-between; }
.paragraph-border-settings__color input { border: 1px solid var(--p-content-border-color); border-radius: 3px; height: 1.7rem; padding: .1rem; width: 2.4rem; }
.paragraph-border-settings__number :deep(.p-inputnumber) { width: 5rem; }
.paragraph-border-settings__number :deep(.p-inputnumber-input) { font-size: .72rem; min-width: 0; padding: .3rem .15rem; text-align: center; }
.recipe-paragraph__heading { align-items: center; color: var(--p-primary-700); display: flex; font-size: .9rem; font-weight: 700; gap: .45rem; margin-bottom: .55rem; }
.recipe-paragraph__heading > i { flex: 0 0 auto; }
.recipe-paragraph__title { background: transparent; border: 0; color: inherit; font: inherit; min-width: 0; outline: none; width: 100%; }
.recipe-paragraph__title:focus { box-shadow: 0 1px 0 var(--p-primary-color); }
.recipe-paragraph__content { color: var(--p-text-color); font-size: 1rem; line-height: 1.7; margin: 0; min-height: 1.7em; }
.recipe-steps__add { margin-top: .5rem; }
@media (max-width: 850px) {
    .recipe-paragraph-block { padding: .7rem .75rem; }
}
</style>