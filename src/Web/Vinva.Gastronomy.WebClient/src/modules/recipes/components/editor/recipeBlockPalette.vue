<template>
    <aside
        class="block-sidebar"
        :class="{ 'is-collapsed': collapsed }"
        aria-label="Палитра блоков рецепта"
        @pointerdown.capture="emit('pointerdown', $event)"
    >
        <div class="sidebar-heading">
            <div v-if="!collapsed">
                <small>ПАЛИТРА</small>
                <strong>Добавить блок</strong>
            </div>
            <span v-if="!collapsed" class="block-count" :aria-label="`${blockCount} блоков в документе`">{{ blockCount }}</span>
            <Button
                :icon="collapsed ? 'pi pi-angle-right' : 'pi pi-angle-left'"
                text
                severity="secondary"
                size="small"
                class="sidebar-toggle"
                :aria-label="collapsed ? 'Развернуть палитру блоков' : 'Свернуть палитру блоков'"
                :aria-expanded="!collapsed"
                v-tooltip.right="collapsed ? 'Развернуть палитру блоков' : 'Свернуть палитру блоков'"
                @click="emit('update:collapsed', !collapsed)"
            />
        </div>
        <label v-if="!collapsed" class="block-search">
            <i class="pi pi-search" />
            <InputText :model-value="search" placeholder="Найти блок" aria-label="Поиск блоков" @update:model-value="emit('update:search', $event)" />
        </label>
        <nav v-if="!collapsed" class="block-categories" aria-label="Категории блоков">
            <Button
                v-for="category in categories"
                :key="category"
                :label="category"
                :text="activeCategory !== category"
                :severity="activeCategory === category ? 'primary' : 'secondary'"
                size="small"
                @click="emit('update:activeCategory', category)"
            />
        </nav>
        <div v-if="groups.length" class="block-groups">
            <section v-for="group in groups" :key="group.label" class="block-group">
                <h2>{{ group.label }}</h2>
                <Button
                    v-for="item in group.items"
                    :key="item.type"
                    class="block-button"
                    :data-block-type="item.type"
                    :aria-label="`${item.label}: ${item.description}`"
                    v-tooltip.right="collapsed ? `${item.label}: ${item.description}` : undefined"
                    text
                    severity="secondary"
                    @click="emit('add-block', item.type)"
                >
                    <span class="block-button__icon"><i :class="item.icon" /></span>
                    <span v-if="!collapsed" class="block-button__copy">
                        <strong>{{ item.label }}</strong>
                        <small>{{ item.description }}</small>
                    </span>
                    <i v-if="!collapsed" class="pi pi-plus block-button__plus" />
                </Button>
            </section>
        </div>
        <p v-else-if="!collapsed" class="block-empty">Блоки не найдены</p>
    </aside>
</template>

<script setup lang="ts">
import type { RecipeBlockType } from '../../recipeBlockCatalog'

type PaletteGroup = {
    label: string
    items: readonly { type: RecipeBlockType; label: string; description: string; icon: string }[]
}

defineProps<{
    collapsed: boolean
    blockCount: number
    categories: string[]
    activeCategory: string
    search: string
    groups: readonly PaletteGroup[]
}>()

const emit = defineEmits(['update:collapsed', 'update:activeCategory', 'update:search', 'add-block', 'pointerdown'])
</script>

<style scoped lang="scss">
.block-sidebar { align-self: start; background: var(--p-surface-0); border: 1px solid var(--p-content-border-color); border-radius: var(--p-border-radius-md, .5rem); max-height: calc(100vh - 6rem); overflow: auto; padding: .9rem; position: sticky; top: 1rem; }
.block-sidebar.is-collapsed { padding: .5rem; }
.block-sidebar.is-collapsed .sidebar-heading { justify-content: center; margin: 0 0 .5rem; }
.block-sidebar.is-collapsed .block-groups { gap: .45rem; }
.block-sidebar.is-collapsed .block-group h2 { display: none; }
.block-sidebar.is-collapsed .block-button { justify-content: center; min-height: 2.6rem; padding: .3rem; }
.block-sidebar.is-collapsed .block-button__icon { flex-basis: 2rem; height: 2rem; }
.sidebar-heading { align-items: center; display: flex; justify-content: space-between; margin: 0 .2rem .8rem; }
.sidebar-heading small, .sidebar-heading strong { display: block; }
.sidebar-heading small { color: var(--p-text-muted-color); font-size: .62rem; font-weight: 700; margin-bottom: .15rem; }
.sidebar-heading strong { font-size: .9rem; }
.sidebar-toggle { flex: 0 0 auto; height: 2rem; padding: 0; width: 2rem; }
.block-count { align-items: center; background: var(--p-primary-50); border: 1px solid var(--p-primary-100); border-radius: 99px; color: var(--p-primary-700); display: inline-flex; font-size: .68rem; height: 1.6rem; justify-content: center; min-width: 1.6rem; padding: 0 .35rem; }
.block-search { align-items: center; border: 1px solid var(--p-content-border-color); border-radius: var(--p-inputtext-border-radius); color: var(--p-text-muted-color); display: flex; gap: .5rem; margin-bottom: .7rem; padding: 0 .65rem; }
.block-search > i { font-size: .75rem; }
.block-search :deep(.p-inputtext) { background: transparent; border: 0; box-shadow: none; font-size: .78rem; min-width: 0; padding: .6rem 0; width: 100%; }
.block-search :deep(.p-inputtext:enabled:focus) { box-shadow: none; }
.block-categories { border-bottom: 1px solid var(--p-content-border-color); display: flex; gap: .15rem; margin: 0 0 .75rem; overflow-x: auto; padding-bottom: .55rem; }
.block-categories :deep(.p-button) { flex: 0 0 auto; font-size: .7rem; padding: .38rem .55rem; }
.block-groups { display: grid; gap: .8rem; }
.block-group h2 { color: var(--p-text-muted-color); font-size: .65rem; font-weight: 700; margin: 0 0 .3rem .45rem; text-transform: uppercase; }
.block-button { align-items: center; border-radius: var(--p-button-border-radius); display: flex; gap: .6rem; justify-content: flex-start; min-height: 44px; padding: .35rem .45rem; text-align: left; width: 100%; }
.block-button:hover { background: var(--p-surface-hover); }
.block-button__icon { align-items: center; background: var(--p-surface-100); border-radius: 4px; color: var(--p-primary-color); display: flex; flex: 0 0 30px; height: 30px; justify-content: center; }
.block-button__icon i { font-size: .82rem; }
.block-button__copy { flex: 1; min-width: 0; }
.block-button strong, .block-button small { display: block; }
.block-button strong { font-size: .74rem; font-weight: 600; }
.block-button small { color: var(--p-text-muted-color); font-size: .64rem; line-height: 1.3; margin-top: .12rem; }
.block-button__plus { color: var(--p-text-muted-color); font-size: .65rem; }
.block-empty { color: var(--p-text-muted-color); font-size: .78rem; padding: .4rem; }
@media (max-width: 850px) {
    .block-sidebar { margin-bottom: .75rem; max-height: 48vh; padding: .7rem; position: static; }
    .block-sidebar.is-collapsed { max-height: none; padding: .5rem; }
    .block-sidebar.is-collapsed .block-groups { display: flex; flex-wrap: wrap; gap: .2rem; }
    .block-sidebar.is-collapsed .block-group { display: contents; }
    .block-sidebar.is-collapsed .block-button { width: 2.6rem; }
    .block-groups { gap: .65rem; }
    .block-button small { display: none; }
    .block-button strong { font-size: .68rem; line-height: 1.2; }
    .block-button__icon { flex-basis: 28px; height: 28px; }
}
</style>