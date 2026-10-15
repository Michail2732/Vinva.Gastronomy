import { createIngredientRow } from './ingredientListModel'

export const recipeBlockGroups = [
    {
        label: 'Текст',
        items: [
            { type: 'textBlock', label: 'Текст', description: 'Обычный абзац', icon: 'pi pi-align-left' },
            { type: 'taskList', label: 'Чек-лист', description: 'Отмечайте выполненное', icon: 'pi pi-check-square' },
            { type: 'toggle', label: 'Сворачиваемый блок', description: 'Скрыть подробности', icon: 'pi pi-chevron-down' },
            { type: 'groupBox', label: 'Блок с заголовком', description: 'Заголовок, текст и оформление границы', icon: 'pi pi-info-circle' },
            { type: 'codeBlock', label: 'Код', description: 'Моноширинный блок', icon: 'pi pi-code' },
            { type: 'table', label: 'Таблица', description: 'Строки и столбцы', icon: 'pi pi-table' },
            { type: 'horizontalRule', label: 'Разделитель', description: 'Раздел между темами', icon: 'pi pi-minus' },
        ],
    },
    {
        label: 'Рецепт',
        items: [
            { type: 'ingredientList', label: 'Ингредиенты', description: 'Продукты, количества и части', icon: 'pi pi-shopping-bag' },
            { type: 'preparationSteps', label: 'Шаги приготовления', description: 'Последовательность действий', icon: 'pi pi-list-check' },
        ],
    },
    {
        label: 'Медиа',
        items: [
            { type: 'recipeImage', label: 'Изображение', description: 'Один кадр с подписью', icon: 'pi pi-image' },
            { type: 'imageGallery', label: 'Галерея', description: 'Несколько кадров процесса', icon: 'pi pi-images' },
            { type: 'video', label: 'Видео', description: 'Ссылка на видео', icon: 'pi pi-video' },
            { type: 'audio', label: 'Аудио', description: 'Звуковая запись', icon: 'pi pi-volume-up' },
            { type: 'bookmark', label: 'Ссылка', description: 'Ссылка на источник', icon: 'pi pi-link' },
            { type: 'embed', label: 'Встроенный плеер', description: 'YouTube или Vimeo', icon: 'pi pi-window-maximize' },
            { type: 'file', label: 'Файл', description: 'Ссылка на файл рецепта', icon: 'pi pi-paperclip' },
        ],
    },
] as const

export const recipeBlockCatalog = recipeBlockGroups.flatMap((group) => [...group.items]) as unknown as Array<(typeof recipeBlockGroups)[number]['items'][number]>

export type RecipeBlockType = typeof recipeBlockCatalog[number]['type']

export function createRecipeBlockContent(type: RecipeBlockType, tableColumns = 3) {
    switch (type) {
        case 'textBlock':
            return { type: 'textBlock' }
        case 'taskList':
            return { type: 'taskList', content: [{ type: 'taskItem', attrs: { checked: false }, content: [{ type: 'paragraph' }] }] }
        case 'toggle':
            return { type: 'recipeToggle', content: [{ type: 'text', text: 'Добавьте подробности...' }] }
        case 'groupBox':
            return { type: 'groupBox', attrs: { title: 'Параграф', icon: 'pi pi-info-circle' }, content: [{ type: 'paragraph' }] }
        case 'codeBlock':
            return { type: 'codeBlock', content: [{ type: 'text', text: '' }] }
        case 'table': {
            const columnCount = Math.max(1, Math.min(8, Math.floor(tableColumns)))
            return {
                type: 'table',
                content: Array.from({ length: 3 }, (_, rowIndex) => ({
                    type: 'tableRow',
                    content: Array.from({ length: columnCount }, () => ({
                        type: rowIndex === 0 ? 'tableHeader' : 'tableCell',
                        content: [{ type: 'paragraph' }],
                    })),
                })),
            }
        }
        case 'ingredientList':
            return { type, attrs: { ingredients: [createIngredientRow()] } }
        case 'preparationSteps':
            return { type: 'groupBox', attrs: { title: 'Шаги приготовления', icon: 'pi pi-list-check', kind: 'steps' }, content: [{ type: 'text', text: 'Опишите шаг приготовления...' }] }
        case 'recipeImage':
            return { type, attrs: { src: '', alt: 'Изображение рецепта', caption: 'Добавьте подпись к изображению' } }
        case 'imageGallery':
            return { type, attrs: { images: [] } }
        case 'video':
        case 'audio':
        case 'bookmark':
        case 'embed':
        case 'file':
            return { type: 'recipeMedia', attrs: { kind: type, url: '', caption: '' } }
        case 'horizontalRule':
            return { type: 'horizontalRule' }
    }
}
