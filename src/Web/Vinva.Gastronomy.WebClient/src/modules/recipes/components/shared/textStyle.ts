import { TextStyle } from '@tiptap/extension-text-style'

export const RecipeTextStyle = TextStyle.extend({
    addAttributes() {
        return {
            ...this.parent?.(),
            fontSize: {
                default: null as string | null,
                parseHTML: (element: HTMLElement) => element.style.fontSize || null,
                renderHTML: (attributes: { fontSize?: string | null }) => attributes.fontSize ? { style: `font-size: ${attributes.fontSize}` } : {},
            },
            fontFamily: {
                default: null as string | null,
                parseHTML: (element: HTMLElement) => element.style.fontFamily || null,
                renderHTML: (attributes: { fontFamily?: string | null }) => attributes.fontFamily ? { style: `font-family: "${attributes.fontFamily}"` } : {},
            },
        }
    },
})
