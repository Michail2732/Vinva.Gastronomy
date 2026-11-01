<template>
    <Button icon="pi pi-arrows-alt" text severity="secondary" aria-label="Настройка отступов" title="Отступы блока" @mousedown.prevent @click="toggle" />
    <Popover ref="popover">
        <div class="recipe-block-spacing" aria-label="Отступы блока">
            <span class="recipe-block-spacing__label">Отступы, до 100 px</span>
            <label v-for="side in sides" :key="side.field" class="recipe-block-spacing__row">
                <span>{{ side.label }}</span>
                <InputNumber
                    :model-value="current(side.field)"
                    :min="0"
                    :max="100"
                    :step="4"
                    :use-grouping="false"
                    suffix=" px"
                    placeholder="0"
                    :aria-label="`Отступ ${side.label.toLocaleLowerCase()}`"
                    @update:model-value="update(side.field, $event)"
                />
            </label>
            <Button class="recipe-block-spacing__reset" icon="pi pi-undo" text severity="secondary" label="Сбросить отступы" size="small" @mousedown.prevent @click="reset" />
        </div>
    </Popover>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import Popover from 'primevue/popover'
import { MAX_BLOCK_MARGIN } from '../shared/margins'

const sides = [
    { field: 'marginTop', label: 'Сверху' },
    { field: 'marginRight', label: 'Справа' },
    { field: 'marginBottom', label: 'Снизу' },
    { field: 'marginLeft', label: 'Слева' },
] as const

const props = defineProps<{
    attrs: Record<string, unknown>
    updateAttributes: (attributes: Record<string, number>) => void
}>()

const popover = ref<{ toggle: (event: Event) => void; hide: () => void } | null>(null)

function toggle(event: MouseEvent) {
    popover.value?.toggle(event)
}

function clamp(value: unknown) {
    return Math.min(MAX_BLOCK_MARGIN, Math.max(0, Math.round(Number(value) || 0)))
}

function current(field: string) {
    const value = clamp(props.attrs[field])
    return value > 0 ? value : null
}

function update(field: string, value: number | null) {
    props.updateAttributes({ [field]: clamp(value) })
}

function reset() {
    props.updateAttributes({ marginTop: 0, marginRight: 0, marginBottom: 0, marginLeft: 0 })
}
</script>

<style scoped lang="scss">
.recipe-block-spacing { display: grid; gap: .5rem; min-width: 12rem; }
.recipe-block-spacing__label { color: var(--p-text-muted-color); font-size: .72rem; font-weight: 700; }
.recipe-block-spacing__row { align-items: center; color: var(--p-text-muted-color); display: flex; font-size: .75rem; gap: .75rem; justify-content: space-between; }
.recipe-block-spacing__row :deep(.p-inputnumber) { width: 5.5rem; }
.recipe-block-spacing__row :deep(.p-inputnumber-input) { font-size: .72rem; min-width: 0; padding: .3rem .15rem; text-align: center; }
.recipe-block-spacing__reset { justify-content: flex-start; width: 100%; }
</style>
