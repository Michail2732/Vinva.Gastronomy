import { Node, mergeAttributes } from '@tiptap/core'
import type { Component } from 'vue'
import { VueNodeViewRenderer } from '@tiptap/vue-3'
import CharacteristicView from './CharacteristicView.vue'

export const RecipeCharacteristic = Node.create({
    name: 'recipeCharacteristic',
    group: 'block',
    atom: true,
    draggable: true,
    defining: true,
    addAttributes() {
        return {
            characteristicName: {
                default: '',
                parseHTML: (element: HTMLElement) => element.getAttribute('data-characteristic-name') ?? '',
                renderHTML: (attributes: { characteristicName?: string }) => attributes.characteristicName ? { 'data-characteristic-name': attributes.characteristicName } : {},
            },
            characteristicType: {
                default: 'String',
                parseHTML: (element: HTMLElement) => element.getAttribute('data-characteristic-type') ?? 'String',
                renderHTML: (attributes: { characteristicType?: string }) => ({ 'data-characteristic-type': attributes.characteristicType ?? 'String' }),
            },
            measure: {
                default: '',
                parseHTML: (element: HTMLElement) => element.getAttribute('data-measure') ?? '',
                renderHTML: (attributes: { measure?: string }) => attributes.measure ? { 'data-measure': attributes.measure } : {},
            },
            stringValue: {
                default: '',
                parseHTML: (element: HTMLElement) => element.getAttribute('data-string-value') ?? '',
                renderHTML: (attributes: { stringValue?: string }) => attributes.stringValue ? { 'data-string-value': attributes.stringValue } : {},
            },
            numberValue: {
                default: null,
                parseHTML: (element: HTMLElement) => {
                    const raw = element.getAttribute('data-number-value')
                    if (raw === null || raw === '') return null
                    const parsed = Number(raw)
                    return Number.isFinite(parsed) ? parsed : null
                },
                renderHTML: (attributes: { numberValue?: number | null }) => attributes.numberValue !== null && attributes.numberValue !== undefined ? { 'data-number-value': String(attributes.numberValue) } : {},
            },
            rangeMin: {
                default: null,
                parseHTML: (element: HTMLElement) => {
                    const raw = element.getAttribute('data-range-min')
                    if (raw === null || raw === '') return null
                    const parsed = Number(raw)
                    return Number.isFinite(parsed) ? parsed : null
                },
                renderHTML: (attributes: { rangeMin?: number | null }) => attributes.rangeMin !== null && attributes.rangeMin !== undefined ? { 'data-range-min': String(attributes.rangeMin) } : {},
            },
            rangeMax: {
                default: null,
                parseHTML: (element: HTMLElement) => {
                    const raw = element.getAttribute('data-range-max')
                    if (raw === null || raw === '') return null
                    const parsed = Number(raw)
                    return Number.isFinite(parsed) ? parsed : null
                },
                renderHTML: (attributes: { rangeMax?: number | null }) => attributes.rangeMax !== null && attributes.rangeMax !== undefined ? { 'data-range-max': String(attributes.rangeMax) } : {},
            },
            arrayItems: {
                default: [],
                parseHTML: (element: HTMLElement) => {
                    const value = element.getAttribute('data-array-items')
                    if (!value) return []
                    try {
                        const parsed = JSON.parse(value)
                        return Array.isArray(parsed) ? parsed : []
                    } catch {
                        return []
                    }
                },
                renderHTML: () => ({}),
            },
        }
    },
    parseHTML: () => [{ tag: 'div[data-recipe-block="recipeCharacteristic"]' }],
    renderHTML: ({ node, HTMLAttributes }) => {
        const attrs = node.attrs
        let valueText = ''
        switch (attrs.characteristicType) {
            case 'Number':
                valueText = attrs.numberValue !== null && attrs.numberValue !== undefined ? String(attrs.numberValue) : ''
                break
            case 'Range':
                valueText = [attrs.rangeMin, attrs.rangeMax].filter((v) => v !== null && v !== undefined).join(' – ')
                break
            case 'Array':
                valueText = (attrs.arrayItems ?? []).join(', ')
                break
            default:
                valueText = attrs.stringValue ?? ''
        }
        const measure = attrs.measure ? ` ${attrs.measure}` : ''
        return [
            'div',
            mergeAttributes(HTMLAttributes, {
                'data-recipe-block': 'recipeCharacteristic',
                'data-array-items': JSON.stringify(attrs.arrayItems ?? []),
            }),
            ['span', { class: 'characteristic-name' }, attrs.characteristicName || 'Характеристика'],
            ': ',
            ['span', { class: 'characteristic-value' }, `${valueText}${measure}`],
        ]
    },
    addNodeView: () => VueNodeViewRenderer(CharacteristicView as unknown as Component),
})
