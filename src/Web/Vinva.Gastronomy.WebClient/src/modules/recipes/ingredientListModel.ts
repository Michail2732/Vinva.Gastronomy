export interface IngredientQuantityRowViewModel {
    id: string
    amount: string
    unit: string
}

export interface IngredientRowViewModel {
    id: string
    name: string
    part: string
    quantities: IngredientQuantityRowViewModel[]
    note: string
    textStyle?: IngredientTextStyle
}

export interface IngredientTextStyle {
    bold?: boolean
    italic?: boolean
    underline?: boolean
    strike?: boolean
    fontSize?: number
    fontFamily?: string
    color?: string
    backgroundColor?: string
    textAlign?: 'left' | 'center' | 'right'
}

export function createIngredientQuantityRow(): IngredientQuantityRowViewModel {
    return {
        id: globalThis.crypto?.randomUUID?.() ?? `${Date.now()}-${Math.random().toString(36).slice(2)}`,
        amount: '',
        unit: '',
    }
}

export function createIngredientRow(): IngredientRowViewModel {
    return {
        id: globalThis.crypto?.randomUUID?.() ?? `${Date.now()}-${Math.random().toString(36).slice(2)}`,
        name: '',
        part: '',
        note: '',
        quantities: [createIngredientQuantityRow()],
    }
}

function getNodeText(node: { type?: string; text?: string; content?: Array<{ type?: string; text?: string; content?: Array<unknown> }> }): string {
    if (node.type === 'hardBreak') return '\n'
    if (node.text) return node.text
    const children = node.content ?? []
    const separator = children.some((child) => child.type === 'paragraph' || child.type === 'heading') ? '\n' : ''
    return children.map((child) => getNodeText(child as Parameters<typeof getNodeText>[0])).join(separator)
}

function parseLegacyLine(line: string): Pick<IngredientRowViewModel, 'name' | 'quantities'> {
    const delimiterMatch = line.split(/\s+[—–-]\s+/, 2)
    if (delimiterMatch.length === 2)
        return { name: delimiterMatch[0]!.trim(), quantities: [parseLegacyQuantity(delimiterMatch[1]!.trim())] }

    const quantityFirstMatch = line.match(/^(\d+(?:[.,]\d+)?\s+\S+)\s+(.+)$/)
    if (quantityFirstMatch)
        return { name: quantityFirstMatch[2]!.trim(), quantities: [parseLegacyQuantity(quantityFirstMatch[1]!.trim())] }

    return { name: line.trim(), quantities: [createIngredientQuantityRow()] }
}

function parseLegacyQuantity(value: string): IngredientQuantityRowViewModel {
    const match = value.match(/^(\d+(?:[.,]\d+)?)\s*(.*)$/)
    return {
        ...createIngredientQuantityRow(),
        amount: match?.[1] ?? '',
        unit: match?.[2]?.trim() ?? '',
    }
}

export function normalizeIngredientBlock(node: Record<string, unknown>): Record<string, unknown> {
    const attrs = (node.attrs ?? {}) as Record<string, unknown>
    if (Array.isArray(attrs.ingredients)) {
        const ingredients = attrs.ingredients.map((ingredient) => {
            const value = ingredient as Partial<IngredientRowViewModel> & { quantity?: string }
            if (Array.isArray(value.quantities)) {
                return {
                    ...value,
                    id: value.id ?? createIngredientRow().id,
                    name: value.name ?? '',
                    part: value.part ?? '',
                    note: value.note ?? '',
                }
            }
            return {
                id: value.id ?? createIngredientRow().id,
                name: value.name ?? '',
                part: value.part ?? '',
                note: value.note ?? '',
                quantities: [parseLegacyQuantity(value.quantity ?? '')],
            }
        })
        return { ...node, attrs: { ...attrs, ingredients }, content: undefined }
    }

    const legacyText = getNodeText(node as Parameters<typeof getNodeText>[0])
    const ingredients = legacyText
        .split(/\r?\n/)
        .map((line) => line.trim())
        .filter(Boolean)
        .map((line) => ({ id: createIngredientRow().id, ...parseLegacyLine(line) }))

    return {
        ...node,
        attrs: { ...attrs, ingredients: ingredients.length ? ingredients : [createIngredientRow()] },
        content: undefined,
    }
}

