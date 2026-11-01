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
            default: null as number | null,
            parseHTML: (element: HTMLElement) => {
                const value = Number(element.getAttribute('data-width'))
                return Number.isFinite(value) && value > 0 ? value : null
            },
            renderHTML: (attributes: { width?: number | null }) => attributes.width ? { 'data-width': String(Math.round(attributes.width)) } : {},
        },
        height: {
            default: null as number | null,
            parseHTML: (element: HTMLElement) => {
                const value = Number(element.getAttribute('data-height'))
                return Number.isFinite(value) && value > 0 ? value : null
            },
            renderHTML: (attributes: { height?: number | null }) => attributes.height ? { 'data-height': String(Math.round(attributes.height)) } : {},
        },
        stretch: {
            default: 'uniform',
            parseHTML: (element: HTMLElement) => element.getAttribute('data-stretch') || 'uniform',
            renderHTML: (attributes: { stretch?: string }) => ({ 'data-stretch': attributes.stretch || 'uniform' }),
        },
        optimize: {
            default: true,
            parseHTML: (element: HTMLElement) => element.getAttribute('data-optimize') !== 'false',
            renderHTML: (attributes: { optimize?: boolean }) => ({ 'data-optimize': String(attributes.optimize !== false) }),
        },
    }),
    parseHTML: () => [{ tag: 'figure[data-recipe-block="recipeImage"]' }],
    renderHTML: ({ HTMLAttributes }) => {
        const width = Number(HTMLAttributes['data-width']) || null
        const height = Number(HTMLAttributes['data-height']) || null
        const stretch = String(HTMLAttributes['data-stretch'] || 'uniform')
        const objectFit = stretch === 'fill' ? 'fill' : stretch === 'uniformToFill' ? 'cover' : 'contain'
        const style = `width: ${width ? `${width}px` : '100%'};${height ? ` height: ${height}px;` : ''} object-fit: ${objectFit};`
        return [
            'figure',
            mergeAttributes(HTMLAttributes, { 'data-recipe-block': 'recipeImage' }),
            ['img', {
                src: HTMLAttributes.src,
                alt: HTMLAttributes.alt,
                loading: HTMLAttributes['data-optimize'] === 'false' ? 'eager' : 'lazy',
                decoding: HTMLAttributes['data-optimize'] === 'false' ? 'sync' : 'async',
                style,
            }],
            ['figcaption', {}, HTMLAttributes.caption],
        ]
    },
    addNodeView: () => VueNodeViewRenderer(ImageView as unknown as Component),
})
