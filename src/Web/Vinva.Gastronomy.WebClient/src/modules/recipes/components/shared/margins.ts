import { Extension } from '@tiptap/core'

export const MAX_BLOCK_MARGIN = 100

export const blockMarginTypes = [
    'groupBox',
    'textBlock',
    'taskList',
    'recipeToggle',
    'table',
    'horizontalRule',
    'ingredientsSet',
    'recipeCharacteristic',
    'recipeImage',
    'imageGallery',
    'recipeMedia',
] as const

const marginSides = ['Top', 'Right', 'Bottom', 'Left'] as const
export type MarginSide = (typeof marginSides)[number]

export function marginField(side: MarginSide) {
    return `margin${side}` as const
}

function marginAttribute(side: MarginSide) {
    const field = marginField(side)
    const dataField = `data-margin-${side.toLowerCase()}`
    return {
        default: 0,
        parseHTML: (element: HTMLElement) => {
            const value = Number(element.getAttribute(dataField))
            return Number.isFinite(value) && value > 0 ? Math.min(MAX_BLOCK_MARGIN, value) : 0
        },
        renderHTML: (attributes: Record<string, number>) => {
            const value = Number(attributes[field])
            return Number.isFinite(value) && value > 0 ? { [dataField]: String(Math.min(MAX_BLOCK_MARGIN, Math.round(value))) } : {}
        },
    }
}

export const RecipeBlockMargins = Extension.create({
    name: 'recipeBlockMargins',
    addGlobalAttributes() {
        return [{
            types: [...blockMarginTypes],
            attributes: {
                marginTop: marginAttribute('Top'),
                marginRight: marginAttribute('Right'),
                marginBottom: marginAttribute('Bottom'),
                marginLeft: marginAttribute('Left'),
            },
        }]
    },
})

export function blockMarginStyle(attrs: Record<string, unknown>): Record<string, string> {
    const style: Record<string, string> = {}
    for (const side of marginSides) {
        const value = Number(attrs[marginField(side)])
        if (Number.isFinite(value) && value > 0) style[marginField(side)] = `${Math.min(MAX_BLOCK_MARGIN, Math.round(value))}px`
    }
    return style
}
