import type { JSONContent } from '@tiptap/core'
import type { RecipeViewModel } from './types/recipeTypes'
import { normalizeIngredientBlock } from './ingredientListModel'

const legacyParagraphDefaults: Record<string, { title: string; icon: string }> = {
    recipeComment: { title: 'Комментарий автора', icon: 'pi pi-comment' },
    recipeCallout: { title: 'Выноска', icon: 'pi pi-lightbulb' },
    blockquote: { title: 'Цитата', icon: 'pi pi-comment' },
    preparationSteps: { title: 'Шаги приготовления', icon: 'pi pi-list-check' },
}

const inlineContentTypes = new Set(['text', 'hardBreak'])

function ensureBlockContent(content: JSONContent[] | undefined): JSONContent[] | undefined {
    if (!content?.length) return content
    if (!content.some((child) => child.type && inlineContentTypes.has(child.type))) return content
    const blocks: JSONContent[] = []
    let inlineRun: JSONContent[] = []
    const flushInlineRun = () => {
        if (inlineRun.length) {
            blocks.push({ type: 'paragraph', content: inlineRun })
            inlineRun = []
        }
    }
    for (const child of content) {
        if (child.type && inlineContentTypes.has(child.type)) inlineRun.push(child)
        else {
            flushInlineRun()
            blocks.push(child)
        }
    }
    flushInlineRun()
    return blocks
}

function normalizeRecipeNode(node: JSONContent, isTopLevel = false): JSONContent {
    const defaults = node.type ? legacyParagraphDefaults[node.type] : undefined
    let normalized = node

    if (defaults) {
        const attrs = node.attrs ?? {}
        const content = node.type === 'blockquote'
            ? (node.content ?? []).flatMap((child) => child.type === 'paragraph' ? child.content ?? [] : [child])
            : node.content
        normalized = {
            type: 'groupBox',
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
            content: ensureBlockContent(content),
        }
    }

    const renameMap: Record<string, string> = { recipeParagraph: 'groupBox', recipeText: 'textBlock' }
    if (normalized.type && renameMap[normalized.type]) {
        normalized = { ...normalized, type: renameMap[normalized.type] }
    }

    if (isTopLevel && normalized.type === 'paragraph') {
        normalized = { type: 'textBlock', ...(normalized.content ? { content: normalized.content } : {}) }
    }

    if (normalized.type === 'groupBox' && normalized.content) {
        normalized = { ...normalized, content: ensureBlockContent(normalized.content) }
    }

    if (normalized.type === 'ingredientsSet') normalized = normalizeIngredientBlock(normalized as unknown as Record<string, unknown>) as unknown as JSONContent
    if (normalized.type === 'recipeImage') {
        const attrs = { ...(normalized.attrs ?? {}) }
        if (attrs.height === undefined && attrs.stretch === undefined) {
            const legacyWidth = Number(attrs.width)
            attrs.width = Number.isFinite(legacyWidth) && legacyWidth > 0 && legacyWidth < 100 ? Math.round((legacyWidth / 100) * 640) : null
            attrs.height = null
            attrs.stretch = 'uniform'
        }
        normalized = { ...normalized, attrs }
    }
    if (normalized.content) normalized = { ...normalized, content: normalized.content.map((child) => normalizeRecipeNode(child)) }
    return normalized
}

export function normalizeRecipeDocument(document: JSONContent): JSONContent {
    return { ...document, content: document.content?.map((node) => normalizeRecipeNode(node, true)) }
}

export function createRecipeDocument(recipe?: Partial<RecipeViewModel> | null): JSONContent {
    if (recipe?.document?.type === 'doc') return normalizeRecipeDocument(recipe.document)

    const content: JSONContent[] = []

    if (recipe?.description) {
        content.push({ type: 'textBlock', content: [{ type: 'text', text: recipe.description }] })
    }

    if (recipe?.ingredientsSets?.length) {
        for (const set of recipe.ingredientsSets) {
            const rows = set.ingredients.map((ingredient) => ({
                id: globalThis.crypto?.randomUUID?.() ?? `${Date.now()}-${Math.random().toString(36).slice(2)}`,
                name: ingredient.name,
                note: '',
                quantities: ingredient.quantities.length
                    ? ingredient.quantities.map((quantity) => ({
                        id: globalThis.crypto?.randomUUID?.() ?? `${Date.now()}-${Math.random().toString(36).slice(2)}`,
                        amount: String(quantity.amount),
                        unit: quantity.unit,
                    }))
                    : [{
                        id: globalThis.crypto?.randomUUID?.() ?? `${Date.now()}-${Math.random().toString(36).slice(2)}`,
                        amount: '',
                        unit: '',
                    }],
            }))
            const portions = set.portionsQuantities[0]
            content.push({
                type: 'ingredientsSet',
                attrs: {
                    title: set.name || 'Ингредиенты',
                    portionsAmount: portions ? String(portions.amount) : '',
                    portionsUnit: portions?.unit ?? 'порций',
                    portionsCalculateType: set.portionsCalculateType,
                    ingredients: rows,
                },
            })
        }
    }

    return { type: 'doc', content: content.length ? content : [{ type: 'textBlock' }] }
}

export const sampleRecipeDocument: JSONContent = {
    type: 'doc',
    content: [
        { type: 'textBlock', content: [{ type: 'text', text: 'Тёплая, шелковистая паста для вечера, когда хочется чего-то уютного.' }] },
        { type: 'recipeComment', content: [{ type: 'text', text: 'Тыкву можно запечь заранее: так вкус получится глубже, а ужин соберётся за двадцать минут.' }] },
        { type: 'ingredientsSet', content: [{ type: 'text', text: '300 г тыквы\n250 г ригатони\n40 г пармезана\n8 листьев шалфея' }] },
        { type: 'preparationSteps', content: [{ type: 'text', text: 'Запеките тыкву до мягкости при 200°C. Сварите пасту al dente. Пробейте тыкву с небольшим количеством воды от пасты, смешайте и завершите пармезаном.' }] },
        { type: 'horizontalRule' },
        { type: 'recipeImage', attrs: { src: '', caption: 'Финальный штрих: шалфей и немного пармезана.' } },
    ],
}
