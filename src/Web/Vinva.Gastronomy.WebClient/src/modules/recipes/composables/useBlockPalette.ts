import { computed, ref } from 'vue'
import { recipeBlockGroups } from '../recipeBlockCatalog'

export function useBlockPalette() {
    const sidebarCollapsed = ref(false)
    const blockSearch = ref('')
    const activeCategory = ref('Все')
    const blockCount = ref(0)

    const blockCategories = ['Все', ...recipeBlockGroups.map((group) => group.label)]
    const visibleBlockGroups = computed(() => {
        const search = blockSearch.value.trim().toLocaleLowerCase('ru')
        return recipeBlockGroups
            .filter((group) => activeCategory.value === 'Все' || group.label === activeCategory.value)
            .map((group) => ({
                ...group,
                items: group.items.filter((item) => !search || `${item.label} ${item.description}`.toLocaleLowerCase('ru').includes(search)),
            }))
            .filter((group) => group.items.length)
    })

    return {
        sidebarCollapsed,
        blockSearch,
        activeCategory,
        blockCount,
        blockCategories,
        visibleBlockGroups,
    }
}
