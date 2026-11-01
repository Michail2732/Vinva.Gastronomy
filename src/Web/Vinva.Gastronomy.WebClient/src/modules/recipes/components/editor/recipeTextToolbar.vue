<template>
    <div class="editor-toolbar" role="toolbar" aria-label="Основная панель управления текстом">
        <div class="toolbar-group">
            <Button text severity="secondary" class="tool-button format-letter format-letter--bold" :class="{ active: isTextMarkActive('bold') }" v-tooltip.top="'Жирный'" aria-label="Жирный" @mousedown.prevent @click="emit('toggle-mark', 'bold')">B</Button>
            <Button text severity="secondary" class="tool-button format-letter format-letter--italic" :class="{ active: isTextMarkActive('italic') }" v-tooltip.top="'Курсив'" aria-label="Курсив" @mousedown.prevent @click="emit('toggle-mark', 'italic')">I</Button>
            <Button text severity="secondary" class="tool-button format-letter format-letter--underline" :class="{ active: isTextMarkActive('underline') }" v-tooltip.top="'Подчеркнутый текст'" aria-label="Подчеркнутый текст" @mousedown.prevent @click="emit('toggle-mark', 'underline')">U</Button>
            <Button text severity="secondary" class="tool-button format-letter format-letter--strike" :class="{ active: isTextMarkActive('strike') }" v-tooltip.top="'Зачеркнутый текст'" aria-label="Зачеркнутый текст" @mousedown.prevent @click="emit('toggle-mark', 'strike')">S</Button>
            <Button icon="pi pi-list" text severity="secondary" class="tool-button" :class="{ active: editor.isActive('bulletList') }" v-tooltip.top="'Маркированный список'" aria-label="Маркированный список" @click="editor.chain().focus().toggleBulletList().run()" />
            <Button icon="pi pi-list-check" text severity="secondary" class="tool-button" :class="{ active: editor.isActive('orderedList') }" v-tooltip.top="'Нумерованный список'" aria-label="Нумерованный список" @click="editor.chain().focus().toggleOrderedList().run()" />
        </div>
        <div class="toolbar-divider" />
        <div class="toolbar-group" aria-label="Выравнивание текста">
            <Button icon="pi pi-align-left" text severity="secondary" class="tool-button" :class="{ active: isTextAligned('left') }" v-tooltip.top="'По левому краю'" aria-label="По левому краю" @mousedown.prevent @click="emit('align', 'left')" />
            <Button icon="pi pi-align-center" text severity="secondary" class="tool-button" :class="{ active: isTextAligned('center') }" v-tooltip.top="'По центру'" aria-label="По центру" @mousedown.prevent @click="emit('align', 'center')" />
            <Button icon="pi pi-align-right" text severity="secondary" class="tool-button" :class="{ active: isTextAligned('right') }" v-tooltip.top="'По правому краю'" aria-label="По правому краю" @mousedown.prevent @click="emit('align', 'right')" />
        </div>
        <div class="toolbar-divider" />
        <div class="text-style-control">
            <Button icon="pi pi-minus" text severity="secondary" class="font-step-button" v-tooltip.top="'Уменьшить размер шрифта'" aria-label="Уменьшить размер шрифта" @mousedown.prevent @click="emit('font-step', -1)" />
            <InputNumber :model-value="fontSize" :min="8" :max="72" suffix=" px" :use-grouping="false" aria-label="Размер шрифта" @update:model-value="emit('update:fontSize', $event)" />
            <Button icon="pi pi-plus" text severity="secondary" class="font-step-button" v-tooltip.top="'Увеличить размер шрифта'" aria-label="Увеличить размер шрифта" @mousedown.prevent @click="emit('font-step', 1)" />
            <Button text severity="secondary" class="font-step-button heading-picker-button" v-tooltip.top="'Стиль заголовка'" aria-label="Выбрать стиль заголовка" @mousedown.prevent @click="toggleHeadingPopover">
                <svg viewBox="0 0 24 24" width="14" height="14" fill="currentColor" aria-hidden="true"><path d="M5 4v3h5.5v12h3V7H19V4z"/></svg>
            </Button>
            <Popover ref="headingPopover">
                <div class="heading-picker-options" aria-label="Уровень заголовка">
                    <Button label="Без стиля" text severity="secondary" @click="selectHeading(0)" />
                    <Button v-for="heading in headingOptions" :key="heading.level" :label="heading.label" text severity="secondary" @click="selectHeading(heading.level)" />
                </div>
            </Popover>
        </div>
        <Select
            :model-value="fontFamily"
            :options="fontFamilies"
            option-label="label"
            option-value="value"
            aria-label="Шрифт текста"
            class="font-family-select"
            @mousedown.prevent
            @update:model-value="emit('update:fontFamily', $event)"
        >
            <template #value="{ value }">
                <span class="font-preview"><span class="font-preview__sample" :style="fontStyle(value)">Ab</span><span>{{ fontFamilyLabel(value) }}</span></span>
            </template>
            <template #option="{ option }">
                <span class="font-preview"><span class="font-preview__sample" :style="fontStyle(option.value)">Ab</span><span>{{ option.label }}</span></span>
            </template>
        </Select>
        <label class="text-color-control" v-tooltip.top="'Цвет текста'">
            <span class="word-color-icon word-color-icon--text" aria-hidden="true">
                <svg viewBox="0 0 24 24" width="16" height="16"><path d="M11 3L5.5 17h2.1l1.1-3h6.6l1.1 3h2.1L13 3h-2zm1 4.5L14 12h-4l2-4.5z" fill="currentColor"/><rect x="4" y="19" width="16" height="2.5" rx="1" class="word-color-bar"/></svg>
            </span>
            <input type="color" aria-label="Цвет текста" :value="fontColor || '#202522'" @mousedown.prevent @input="emit('update:fontColor', ($event.target as HTMLInputElement).value)" />
        </label>
        <label class="text-color-control" v-tooltip.top="'Цвет фона текста'">
            <span class="word-color-icon word-color-icon--highlight" aria-hidden="true">
                <svg viewBox="0 0 24 24" width="16" height="16"><path d="M11 3L5.5 17h2.1l1.1-3h6.6l1.1 3h2.1L13 3h-2zm1 4.5L14 12h-4l2-4.5z" fill="currentColor"/><rect x="4" y="18.5" width="16" height="3" rx="1" class="word-color-bar"/></svg>
            </span>
            <input type="color" aria-label="Цвет фона текста" :value="fontBackgroundColor || '#ffffff'" @mousedown.prevent @input="emit('update:fontBackgroundColor', ($event.target as HTMLInputElement).value)" />
        </label>
        <div class="toolbar-divider" />
        <Button icon="pi pi-face-smile" text severity="secondary" class="tool-button" v-tooltip.top="'Вставить эмодзи'" aria-label="Вставить эмодзи" @mousedown.prevent @click="toggleEmojiPopover" />
        <Popover ref="emojiPopover">
            <div class="emoji-picker" aria-label="Выбор эмодзи">
                <InputText :model-value="emojiSearch" placeholder="Найти эмодзи" aria-label="Поиск эмодзи" class="emoji-picker__search" @update:model-value="emojiSearch = $event ?? ''" />
                <div class="emoji-picker__grid">
                    <button
                        v-for="item in filteredEmojis"
                        :key="item.emoji"
                        type="button"
                        class="emoji-picker__item"
                        :aria-label="item.label"
                        :title="item.label"
                        @click="selectEmoji(item.emoji)"
                    >{{ item.emoji }}</button>
                </div>
                <p v-if="!filteredEmojis.length" class="emoji-picker__empty">Ничего не найдено</p>
            </div>
        </Popover>
        <div class="toolbar-divider" />
        <Button icon="pi pi-undo" text severity="secondary" class="tool-button" v-tooltip.top="'Отменить'" aria-label="Отменить" @click="editor.chain().focus().undo().run()" />
        <Button icon="pi pi-refresh" text severity="secondary" class="tool-button" v-tooltip.top="'Повторить'" aria-label="Повторить" @click="editor.chain().focus().redo().run()" />
        <div class="toolbar-divider" />
        <Button icon="pi pi-arrow-up" text severity="secondary" class="tool-button" :disabled="!canMoveBlock(-1)" v-tooltip.top="'Переместить блок вверх'" aria-label="Переместить блок вверх" @click="emit('move-block', -1)" />
        <Button icon="pi pi-arrow-down" text severity="secondary" class="tool-button" :disabled="!canMoveBlock(1)" v-tooltip.top="'Переместить блок вниз'" aria-label="Переместить блок вниз" @click="emit('move-block', 1)" />
        <span class="toolbar-spacer" />
    </div>
</template>

<script setup lang="ts">
import type { Editor } from '@tiptap/core'
import { computed, ref } from 'vue'
import { NodeSelection, TextSelection } from '@tiptap/pm/state'
import Popover from 'primevue/popover'
import Select from 'primevue/select'
import { searchEmojis } from './emojiData'

type FontOption = { label: string; value: string }
type TextAlignment = 'left' | 'center' | 'right'
type TextMark = 'bold' | 'italic' | 'underline' | 'strike'
type HeadingLevel = 0 | 1 | 2 | 3

const headingOptions: { level: Exclude<HeadingLevel, 0>; label: string }[] = [
    { level: 1, label: 'Заголовок 1' },
    { level: 2, label: 'Заголовок 2' },
    { level: 3, label: 'Заголовок 3' },
]

const props = defineProps<{
    editor: Editor
    fontSize: number
    fontColor: string
    fontBackgroundColor: string
    fontFamily: string
    fontFamilies: FontOption[]
    canMoveBlock: (direction: -1 | 1) => boolean
    isTextAligned: (alignment: TextAlignment) => boolean
    isTextMarkActive: (mark: TextMark) => boolean
}>()

const emit = defineEmits<{
    'update:fontSize': [value: number | null]
    'update:fontColor': [value: string]
    'update:fontBackgroundColor': [value: string]
    'update:fontFamily': [value: string]
    'toggle-mark': [mark: TextMark]
    align: [alignment: TextAlignment]
    'font-step': [direction: -1 | 1]
    'move-block': [direction: -1 | 1]
    'heading-level': [level: HeadingLevel]
}>()

const headingPopover = ref<{ toggle: (event: Event) => void; hide: () => void } | null>(null)
const emojiPopover = ref<{ toggle: (event: Event) => void; hide: () => void } | null>(null)
const emojiSearch = ref('')
const filteredEmojis = computed(() => searchEmojis(emojiSearch.value))

function toggleEmojiPopover(event: MouseEvent) {
    emojiPopover.value?.toggle(event)
}

function selectEmoji(emoji: string) {
    const { state, view } = props.editor
    // Курсор в нативном инпуте NodeView (заголовок блока, ингредиент и т.п.):
    // вставляем эмодзи в этот инпут, не трогая документ.
    const dom = view.dom.ownerDocument.activeElement
    if (dom instanceof HTMLInputElement || dom instanceof HTMLTextAreaElement) {
        const start = dom.selectionStart ?? dom.value.length
        const end = dom.selectionEnd ?? dom.value.length
        dom.value = dom.value.slice(0, start) + emoji + dom.value.slice(end)
        dom.dispatchEvent(new Event('input', { bubbles: true }))
        const caret = start + emoji.length
        dom.setSelectionRange(caret, caret)
        emojiPopover.value?.hide()
        return
    }
    if (state.selection instanceof NodeSelection) {
        // Блок выделен целиком: заменять его эмодзи нельзя — ставим курсор после блока.
        const paragraph = state.schema.nodes.paragraph
        const insertPosition = state.selection.to
        const transaction = state.tr.insert(insertPosition, paragraph ? paragraph.create() : state.schema.text(' '))
        transaction.setSelection(TextSelection.create(transaction.doc, insertPosition + 1))
        view.dispatch(transaction)
    }
    props.editor.chain().focus().insertContent(emoji).run()
    emojiPopover.value?.hide()
}

function toggleHeadingPopover(event: MouseEvent) {
    headingPopover.value?.toggle(event)
}

function selectHeading(level: HeadingLevel) {
    emit('heading-level', level)
    headingPopover.value?.hide()
}

function fontStyle(value: string | null | undefined) {
    return value ? { fontFamily: `'${value}', sans-serif` } : undefined
}

function fontFamilyLabel(value: string | null | undefined) {
    return props.fontFamilies.find((font) => font.value === (value ?? ''))?.label ?? 'По умолчанию'
}
</script>

<style scoped lang="scss">
.editor-toolbar { align-items: center; background: var(--p-surface-0); border: 1px solid var(--p-content-border-color); border-radius: var(--p-border-radius-md, .5rem); color: var(--p-text-color); display: flex; gap: .12rem; min-height: 48px; padding: .35rem .5rem; position: sticky; top: .5rem; z-index: 2; }
.toolbar-group { display: flex; gap: .1rem; }
.toolbar-divider { background: var(--p-content-border-color); height: 22px; margin: 0 .2rem; width: 1px; }
.tool-button { border-radius: 4px; height: 2rem; padding: 0; width: 2rem; }
.tool-button:hover, .tool-button.active { background: var(--p-primary-50); color: var(--p-primary-700); }
.format-letter { font-family: Georgia, 'Times New Roman', serif; font-size: 1rem; }
.format-letter--bold { font-weight: 700; }
.format-letter--italic { font-style: italic; }
.format-letter--underline { text-decoration: underline; text-underline-offset: 2px; }
.format-letter--strike { text-decoration: line-through; }
.text-style-control, .text-color-control { align-items: center; color: var(--p-text-muted-color); display: inline-flex; flex: 0 0 auto; font-size: .72rem; gap: .15rem; }
.text-style-control :deep(.p-inputnumber) { flex: 0 0 2.8rem; min-width: 0; width: 2.8rem; }
.text-style-control :deep(.p-inputnumber-input) { font-size: .7rem; min-width: 0; padding: .3rem 0; text-align: center; }
.font-step-button { flex: 0 0 1.25rem; height: 1.65rem; padding: 0; width: 1.25rem; }
.heading-picker-button { color: var(--p-primary-700); }
.heading-picker-options { display: grid; min-width: 8rem; }
.heading-picker-options :deep(.p-button) { justify-content: flex-start; }
.font-step-button :deep(.p-button-icon) { font-size: .62rem; }
.font-family-select { flex: 0 1 8.5rem; max-width: 8.5rem; min-width: 5rem; }
.font-family-select :deep(.p-select-label) { font-size: .7rem; padding: .3rem .4rem; }
.font-preview { align-items: center; display: inline-flex; gap: .4rem; min-width: 0; }
.font-preview__sample { font-size: 1rem; }
.text-color-control input { background: transparent; border: 1px solid var(--p-content-border-color); border-radius: 4px; cursor: pointer; height: 1.8rem; padding: .15rem; width: 2rem; }
.word-color-icon { align-items: center; display: inline-flex; justify-content: center; }
.word-color-icon .word-color-bar { fill: currentColor; }
.word-color-icon--text .word-color-bar { fill: var(--p-primary-color); }
.word-color-icon--highlight .word-color-bar { fill: #ffe58f; }
.toolbar-spacer { flex: 1; }
.emoji-picker { display: grid; gap: .5rem; width: 17rem; }
.emoji-picker__search :deep(.p-inputtext), .emoji-picker__search { font-size: .78rem; width: 100%; }
.emoji-picker__grid { display: grid; gap: .1rem; grid-template-columns: repeat(8, 1fr); max-height: 14rem; overflow-y: auto; }
.emoji-picker__item { align-items: center; background: transparent; border: 0; border-radius: 4px; cursor: pointer; display: inline-flex; font-size: 1.15rem; height: 1.9rem; justify-content: center; padding: 0; }
.emoji-picker__item:hover { background: var(--p-primary-50); }
.emoji-picker__empty { color: var(--p-text-muted-color); font-size: .75rem; margin: 0; text-align: center; }
@media (max-width: 850px) {
    .editor-toolbar { flex-wrap: wrap; gap: .12rem; padding: .4rem; }
    .toolbar-spacer { display: none; }
}
</style>