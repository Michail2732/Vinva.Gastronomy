import { Node, mergeAttributes } from '@tiptap/core'
import { VueNodeViewRenderer } from '@tiptap/vue-3'
import type { Component } from 'vue'
import TextBlockView from './TextBlockView.vue'

export const TextBlock = Node.create({
    name: 'textBlock',
    group: 'block',
    content: 'inline*',
    parseHTML: () => [{ tag: 'div[data-recipe-block="textBlock"]' }],
    renderHTML: ({ node, HTMLAttributes }) => {
        const textAlign = node.attrs.textAlign as string | null
        return [
            'div',
            mergeAttributes(HTMLAttributes, {
                'data-recipe-block': 'textBlock',
                ...(textAlign ? { style: `text-align: ${textAlign}` } : {}),
            }),
            0,
        ]
    },
    addNodeView: () => VueNodeViewRenderer(TextBlockView as unknown as Component),
})
