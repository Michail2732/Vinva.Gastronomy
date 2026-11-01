import { Node, mergeAttributes } from '@tiptap/core'
import type { Component } from 'vue'
import { VueNodeViewRenderer } from '@tiptap/vue-3'
import { titleColorAttribute, borderWidthAttribute } from '../../shared/attributes'
import IngredientsSetView from './IngredientsSetView.vue'

export const IngredientsSet = Node.create({
    name: 'ingredientsSet',
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
            icon: {
                default: 'pi pi-shopping-bag',
                parseHTML: (element: HTMLElement) => element.getAttribute('data-icon') ?? 'pi pi-shopping-bag',
                renderHTML: (attributes: { icon?: string }) => ({ 'data-icon': attributes.icon ?? 'pi pi-shopping-bag' }),
            },
            portionsAmount: {
                default: '',
                parseHTML: (element: HTMLElement) => element.getAttribute('data-portions-amount') ?? '',
                renderHTML: (attributes: { portionsAmount?: string }) => attributes.portionsAmount ? { 'data-portions-amount': attributes.portionsAmount } : {},
            },
            portionsUnit: {
                default: 'порций',
                parseHTML: (element: HTMLElement) => element.getAttribute('data-portions-unit') ?? 'порций',
                renderHTML: (attributes: { portionsUnit?: string }) => ({ 'data-portions-unit': attributes.portionsUnit ?? 'порций' }),
            },
            portionsCalculateType: {
                default: 'Multiplication',
                parseHTML: (element: HTMLElement) => element.getAttribute('data-portions-calculate-type') ?? 'Multiplication',
                renderHTML: (attributes: { portionsCalculateType?: string }) => ({ 'data-portions-calculate-type': attributes.portionsCalculateType ?? 'Multiplication' }),
            },
            borderColor: {
                default: '#b7c8bb',
                parseHTML: (element: HTMLElement) => element.getAttribute('data-border-color') ?? '#b7c8bb',
                renderHTML: (attributes: { borderColor?: string }) => ({ 'data-border-color': attributes.borderColor ?? '#b7c8bb' }),
            },
            backgroundColor: {
                default: null,
                parseHTML: (element: HTMLElement) => element.getAttribute('data-background-color'),
                renderHTML: (attributes: { backgroundColor?: string | null }) => attributes.backgroundColor ? { 'data-background-color': attributes.backgroundColor } : {},
            },
            borderTopWidth: borderWidthAttribute('top', 1),
            borderRightWidth: borderWidthAttribute('right', 1),
            borderBottomWidth: borderWidthAttribute('bottom', 1),
            borderLeftWidth: borderWidthAttribute('left', 3),
            borderRadius: {
                default: 6,
                parseHTML: (element: HTMLElement) => Number(element.getAttribute('data-border-radius') ?? 6),
                renderHTML: (attributes: { borderRadius?: number }) => ({ 'data-border-radius': String(attributes.borderRadius ?? 6) }),
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
    parseHTML: () => [{ tag: 'section[data-recipe-block="ingredientsSet"]' }, { tag: 'section[data-recipe-block="ingredientList"]' }, { tag: 'div[data-recipe-block="ingredientList"]' }],
    renderHTML: ({ node, HTMLAttributes }) => {
        const borderWidth = [node.attrs.borderTopWidth, node.attrs.borderRightWidth, node.attrs.borderBottomWidth, node.attrs.borderLeftWidth]
            .map((value, index) => `${Math.max(0, Number(value ?? (index === 3 ? 3 : 1)))}px`)
            .join(' ')
        const style = [
            'border-style: solid',
            `border-color: ${node.attrs.borderColor || '#b7c8bb'}`,
            `border-width: ${borderWidth}`,
            `border-radius: ${Math.max(0, Number(node.attrs.borderRadius ?? 6))}px`,
            node.attrs.backgroundColor ? `background-color: ${node.attrs.backgroundColor}` : '',
        ].filter(Boolean).join('; ')
        const portions = [node.attrs.portionsAmount, node.attrs.portionsUnit].filter(Boolean).join(' ')
        return [
            'section',
            mergeAttributes(HTMLAttributes, {
                'data-recipe-block': 'ingredientsSet',
                'data-ingredients': JSON.stringify(node.attrs.ingredients ?? []),
                style,
            }),
            ['div', { class: 'group-box__heading', style: node.attrs.titleColor ? `color: ${node.attrs.titleColor}` : '' },
                ['i', { class: node.attrs.icon || 'pi pi-shopping-bag' }],
                ['strong', {}, node.attrs.title || 'Ингредиенты']],
            ...(portions ? [['p', { class: 'ingredient-portions' }, `Количество порций: ${portions}`] as const] : []),
            ['ul', {}, ...(node.attrs.ingredients ?? []).map((ingredient: { name: string; quantities?: Array<{ amount?: string; unit?: string }>; quantity?: string }) => {
                const quantities = Array.isArray(ingredient.quantities)
                    ? ingredient.quantities.map((quantity) => [quantity.amount, quantity.unit].filter(Boolean).join(' ')).filter(Boolean).join(', ')
                    : (ingredient.quantity ?? '')
                return ['li', {}, [ingredient.name, quantities].filter(Boolean).join(' - ')]
            })],
        ]
    },
    addNodeView: () => VueNodeViewRenderer(IngredientsSetView as unknown as Component),
})
