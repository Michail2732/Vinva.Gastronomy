import { Node, mergeAttributes } from '@tiptap/core'
import type { Component } from 'vue'
import { VueNodeViewRenderer } from '@tiptap/vue-3'
import { titleColorAttribute } from '../../shared/attributes'
import IngredientView from './IngredientView.vue'

export const IngredientList = Node.create({
    name: 'ingredientList',
    group: 'block',
    atom: true,
    draggable: true,
    defining: true,
    addAttributes() {
        return {
            ...titleColorAttribute(),
            title: {
                default: 'Ингредиенты',
                parseHTML: (element: HTMLElement) => element.getAttribute('data-title') ?? 'Ингредиенты',
                renderHTML: (attributes: { title: string }) => ({ 'data-title': attributes.title }),
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
            ingredients: {
                default: [],
                parseHTML: (element: HTMLElement) => {
                    const value = element.getAttribute('data-ingredients')
                    if (!value) return []
                    try {
                        return JSON.parse(value)
                    } catch {
                        return []
                    }
                },
                renderHTML: () => ({}),
            },
        }
    },
    parseHTML: () => [{ tag: 'div[data-recipe-block="ingredientList"]' }],
    renderHTML: ({ node, HTMLAttributes }) => [
        'div',
        mergeAttributes(HTMLAttributes, {
            'data-recipe-block': 'ingredientList',
            'data-ingredients': JSON.stringify(node.attrs.ingredients ?? []),
        }),
        ['ul', {}, ...(node.attrs.ingredients ?? []).map((ingredient: { name: string; quantity: string }) => [
            'li', {}, [ingredient.name, ingredient.quantity].filter(Boolean).join(' - '),
        ])],
    ],
    addNodeView: () => VueNodeViewRenderer(IngredientView as unknown as Component),
})
