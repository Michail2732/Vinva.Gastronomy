import type { JSONContent } from '@tiptap/core'
import type { RecipeViewModel } from './types/recipeTypes'
import { normalizeIngredientBlock } from './ingredientListModel'

const legacyParagraphDefaults: Record<string, { title: string; icon: string }> = {
    recipeComment: { title: 'Комментарий автора', icon: 'pi pi-comment' },
    recipeCallout: { title: 'Выноска', icon: 'pi pi-lightbulb' },
    blockquote: { title: 'Цитата', icon: 'pi pi-comment' },
    preparationSteps: { title: 'Шаги приготовления', icon: 'pi pi-list-check' },
}

function normalizeRecipeNode(node: JSONContent): JSONContent {
    const defaults = node.type ? legacyParagraphDefaults[node.type] : undefined
    let normalized = node

    if (defaults) {
        const attrs = node.attrs ?? {}
        const content = node.type === 'blockquote'
            ? (node.content ?? []).flatMap((child) => child.type === 'paragraph' ? child.content ?? [] : [child])
            : node.content
        normalized = {
            type: 'recipeParagraph',
            attrs: {
                ...attrs,
                title: attrs.title ?? defaults.title,
                kind: attrs.kind ?? (node.type === 'preparationSteps' ? 'steps' : 'paragraph'),
                icon: attrs.icon ?? defaults.icon,
                titleColor: attrs.titleColor ?? null,
                borderColor: attrs.borderColor ?? attrs.accentColor ?? '#b7c8bb',
                borderTopWidth: attrs.borderTopWidth ?? 1,
                borderRightWidth: attrs.borderRightWidth ?? 1,
                borderBottomWidth: attrs.borderBottomWidth ?? 1,
                borderLeftWidth: attrs.borderLeftWidth ?? 3,
                borderRadius: attrs.borderRadius ?? 6,
            },
            content,
        }
    }

    if (normalized.type === 'ingredientList') normalized = normalizeIngredientBlock(normalized as unknown as Record<string, unknown>) as unknown as JSONContent
    if (normalized.content) normalized = { ...normalized, content: normalized.content.map(normalizeRecipeNode) }
    return normalized
}

export function normalizeRecipeDocument(document: JSONContent): JSONContent {
    return { ...document, content: document.content?.map(normalizeRecipeNode) }
}

export function createRecipeDocument(recipe?: Partial<RecipeViewModel> | null): JSONContent {
    if (recipe?.document?.type === 'doc') return recipe.document

    const content: JSONContent[] = []

    if (recipe?.description) {
        content.push({ type: 'paragraph', content: [{ type: 'text', text: recipe.description }] })
    }

    if (recipe?.ingredients?.length) {
        const ingredients = recipe.ingredients.map((ingredient) => {
            const quantities = ingredient.quantities
                .map((quantity) => `${quantity.amount} ${quantity.unit}`.trim())
                .join(', ')
            return `${ingredient.name}${quantities ? ` — ${quantities}` : ''}`
        }).join('\n')
        content.push({
            type: 'ingredientList',
            content: [{ type: 'text', text: ingredients }],
        })
    }

    return { type: 'doc', content: content.length ? content : [{ type: 'paragraph' }] }
}

export const sampleRecipeDocument: JSONContent = {
    type: 'doc',
    content: [
        { type: 'paragraph', content: [{ type: 'text', text: 'Тёплая, шелковистая паста для вечера, когда хочется чего-то уютного.' }] },
        { type: 'recipeComment', content: [{ type: 'text', text: 'Тыкву можно запечь заранее: так вкус получится глубже, а ужин соберётся за двадцать минут.' }] },
        { type: 'ingredientList', content: [{ type: 'text', text: '300 г тыквы\n250 г ригатони\n40 г пармезана\n8 листьев шалфея' }] },
        { type: 'preparationSteps', content: [{ type: 'text', text: 'Запеките тыкву до мягкости при 200°C. Сварите пасту al dente. Пробейте тыкву с небольшим количеством воды от пасты, смешайте и завершите пармезаном.' }] },
        { type: 'horizontalRule' },
        { type: 'recipeImage', attrs: { src: '', caption: 'Финальный штрих: шалфей и немного пармезана.' } },
    ],
}
