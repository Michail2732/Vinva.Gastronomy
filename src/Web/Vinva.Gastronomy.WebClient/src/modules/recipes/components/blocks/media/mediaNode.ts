import { Node, mergeAttributes } from '@tiptap/core'
import type { Component } from 'vue'
import { VueNodeViewRenderer } from '@tiptap/vue-3'
import MediaView from './MediaView.vue'

export const RecipeMedia = Node.create({
    name: 'recipeMedia',
    group: 'block',
    atom: true,
    draggable: true,
    addAttributes: () => ({
        kind: { default: 'bookmark' },
        url: { default: '' },
        caption: { default: '' },
    }),
    parseHTML: () => [{ tag: 'div[data-recipe-block="recipeMedia"]' }],
    renderHTML: ({ node, HTMLAttributes }) => [
        'div',
        mergeAttributes(HTMLAttributes, {
            'data-recipe-block': 'recipeMedia',
            'data-kind': node.attrs.kind,
            'data-url': node.attrs.url,
            'data-caption': node.attrs.caption,
        }),
        ['p', {}, [node.attrs.caption || node.attrs.url || '']],
    ],
    addNodeView: () => VueNodeViewRenderer(MediaView as unknown as Component),
})

