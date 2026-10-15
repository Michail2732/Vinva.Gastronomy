import { Node, mergeAttributes } from '@tiptap/core'
import type { Component } from 'vue'
import { VueNodeViewRenderer } from '@tiptap/vue-3'
import { titleColorAttribute } from '../../shared/attributes'
import ToggleView from './ToggleView.vue'

export const RecipeToggle = Node.create({
    name: 'recipeToggle',
    group: 'block',
    content: 'inline*',
    defining: true,
    addAttributes() {
        return {
            ...titleColorAttribute(),
            title: {
                default: 'Раскрывающийся блок',
                parseHTML: (element: HTMLElement) => element.getAttribute('data-title') ?? 'Раскрывающийся блок',
                renderHTML: (attributes: { title?: string }) => ({ 'data-title': attributes.title ?? 'Раскрывающийся блок' }),
            },
            backgroundColor: {
                default: null,
                parseHTML: (element: HTMLElement) => element.getAttribute('data-background-color'),
                renderHTML: (attributes: { backgroundColor?: string | null }) => attributes.backgroundColor ? { 'data-background-color': attributes.backgroundColor } : {},
            },
            accentColor: {
                default: null,
                parseHTML: (element: HTMLElement) => element.getAttribute('data-accent-color'),
                renderHTML: (attributes: { accentColor?: string | null }) => attributes.accentColor ? { 'data-accent-color': attributes.accentColor } : {},
            },
            showAccent: {
                default: true,
                parseHTML: (element: HTMLElement) => element.getAttribute('data-show-accent') !== 'false',
                renderHTML: (attributes: { showAccent?: boolean }) => ({ 'data-show-accent': String(attributes.showAccent !== false) }),
            },
        }
    },
    parseHTML: () => [{ tag: 'div[data-recipe-block="recipeToggle"]' }],
    renderHTML: ({ HTMLAttributes }) => [
        'div',
        mergeAttributes(HTMLAttributes, { 'data-recipe-block': 'recipeToggle' }),
        ['p', 0],
    ],
    addNodeView: () => VueNodeViewRenderer(ToggleView as unknown as Component),
})

