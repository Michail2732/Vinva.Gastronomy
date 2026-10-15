import { Node, mergeAttributes } from '@tiptap/core'
import type { Component } from 'vue'
import { VueNodeViewRenderer } from '@tiptap/vue-3'
import ImageView from './ImageView.vue'

export const RecipeImage = Node.create({
    name: 'recipeImage',
    group: 'block',
    atom: true,
    draggable: true,
    addAttributes: () => ({
        src: { default: '/logo.svg' },
        alt: { default: '' },
        caption: { default: '' },
        width: {
            default: 100,
            parseHTML: (element: HTMLElement) => Number(element.getAttribute('data-width') ?? 100),
            renderHTML: (attributes: { width?: number }) => ({ 'data-width': String(Math.min(100, Math.max(25, Number(attributes.width) || 100))) }),
        },
        optimize: {
            default: true,
            parseHTML: (element: HTMLElement) => element.getAttribute('data-optimize') !== 'false',
            renderHTML: (attributes: { optimize?: boolean }) => ({ 'data-optimize': String(attributes.optimize !== false) }),
        },
    }),
    parseHTML: () => [{ tag: 'figure[data-recipe-block="recipeImage"]' }],
    renderHTML: ({ HTMLAttributes }) => [
        'figure',
        mergeAttributes(HTMLAttributes, { 'data-recipe-block': 'recipeImage' }),
        ['img', {
            src: HTMLAttributes.src,
            alt: HTMLAttributes.alt,
            loading: HTMLAttributes.optimize === false ? 'eager' : 'lazy',
            decoding: HTMLAttributes.optimize === false ? 'sync' : 'async',
            style: `width: ${Math.min(100, Math.max(25, Number(HTMLAttributes.width) || 100))}%;`,
        }],
        ['figcaption', {}, HTMLAttributes.caption],
    ],
    addNodeView: () => VueNodeViewRenderer(ImageView as unknown as Component),
})
