<template>
    <div v-if="editable" class="recipe-node-toolbar">
        <slot />
        <Button
            icon="pi pi-trash"
            text
            severity="danger"
            size="small"
            class="recipe-node-toolbar__delete"
            :aria-label="deleteLabel"
            v-tooltip.top="deleteLabel"
            @mousedown.prevent
            @click="deleteNode"
        />
    </div>
</template>

<script setup lang="ts">
withDefaults(defineProps<{
    editable: boolean
    deleteNode: () => void
    deleteLabel?: string
}>(), {
    deleteLabel: 'Удалить блок',
})
</script>

<style scoped lang="scss">
.recipe-node-toolbar { align-items: center; background: var(--p-surface-0); border: 1px solid var(--p-content-border-color); border-radius: 4px; box-shadow: var(--p-overlay-popover-shadow); display: inline-flex; gap: .15rem; opacity: 0; padding: .15rem; pointer-events: none; position: absolute; right: .4rem; top: .35rem; transition: opacity var(--p-transition-duration); z-index: 3; }
:global(.recipe-block:hover > .recipe-node-toolbar), :global(.recipe-block:focus-within > .recipe-node-toolbar) { opacity: 1; pointer-events: auto; }
.recipe-node-toolbar :deep(.p-button) { flex: 0 0 auto; height: 1.65rem; padding: 0; width: 1.65rem; }
.recipe-node-toolbar__delete:hover { background: var(--p-red-50); }
</style>