<template>
    <header class="recipe-workspace__header">
        <div>
            <p class="eyebrow">Редактор рецепта</p>
            <InputText :model-value="title" class="recipe-title" placeholder="Название рецепта" aria-label="Название рецепта" @update:model-value="emit('update:title', $event ?? '')" />
            <p class="recipe-subtitle">Соберите рецепт из текста, ингредиентов и медиа.</p>
        </div>
        <div class="header-actions">
            <span class="save-status"><i :class="saveStatusIcon" /> {{ saveStatus }}</span>
            <Button :label="preview ? 'Редактировать' : 'Предпросмотр'" :icon="preview ? 'pi pi-pencil' : 'pi pi-eye'" text severity="secondary" size="small" @click="emit('update:preview', !preview)" />
            <Button :label="isSaving ? 'Сохраняем...' : 'Сохранить'" :icon="isSaving ? 'pi pi-spin pi-spinner' : 'pi pi-cloud-upload'" :disabled="isSaving" @click="emit('save')" />
        </div>
    </header>
</template>

<script setup lang="ts">
defineProps<{
    title: string
    preview: boolean
    saveStatus: string
    saveStatusIcon: string
    isSaving: boolean
}>()

const emit = defineEmits<{
    'update:title': [value: string]
    'update:preview': [value: boolean]
    save: []
}>()
</script>

<style scoped lang="scss">
.recipe-workspace__header { align-items: center; background: var(--p-surface-0); border: 1px solid var(--p-content-border-color); border-radius: var(--p-border-radius-md, .5rem); display: flex; justify-content: space-between; gap: 1rem; max-width: 1500px; margin: 0 auto 1rem; min-width: 0; padding: 1rem 1.25rem; }
.recipe-workspace__header > div:first-child { flex: 1 1 auto; min-width: 0; }
.eyebrow { color: var(--p-primary-color); font-size: .68rem; font-weight: 700; margin: 0 0 .4rem; text-transform: uppercase; }
.recipe-title { background: transparent; border: 0; box-shadow: none; color: var(--p-text-color); font-size: 1.7rem; font-weight: 700; line-height: 1.2; max-width: 750px; outline: none; padding: 0; width: min(72vw, 750px); }
.recipe-title::placeholder { color: var(--p-form-field-placeholder-color); }
.recipe-title:enabled:focus { box-shadow: none; }
.recipe-subtitle { color: var(--p-text-muted-color); font-size: .82rem; margin: .4rem 0 0; }
.header-actions { align-items: center; display: flex; flex: 0 1 auto; flex-wrap: wrap; gap: .5rem; justify-content: flex-end; min-width: 0; }
.header-actions :deep(.p-button) { flex: 0 0 auto; max-width: 100%; white-space: nowrap; }
.save-status { color: var(--p-text-muted-color); font-size: .76rem; }
.save-status i { margin-right: .35rem; }
@media (max-width: 850px) {
    .recipe-workspace__header { align-items: stretch; flex-direction: column; gap: 1rem; margin-bottom: 1rem; padding: 1rem; }
    .header-actions { gap: .5rem; justify-content: space-between; min-width: 0; width: 100%; }
    .save-status { font-size: .68rem; overflow: hidden; text-overflow: ellipsis; }
    .recipe-title { font-size: 1.45rem; width: 100%; }
    .header-actions :deep(.p-button) { font-size: .75rem; padding: .55rem .65rem; }
}
</style>