import { Node, mergeAttributes } from '@tiptap/core'
import { VueNodeViewRenderer } from '@tiptap/vue-3'
import type { Component } from 'vue'
import { titleColorAttribute } from '../../shared/attributes'
import { borderWidthAttribute } from '../../shared/attributes'
import GroupBoxView from './GroupBoxView.vue'

export const GroupBox = Node.create({
    name: 'groupBox',
    group: 'block',
    content: 'block+',
    addAttributes() {
        return {
            title: {
                default: 'Параграф',
                parseHTML: (element: HTMLElement) => element.getAttribute('data-title') ?? 'Параграф',
                renderHTML: (attributes: { title?: string }) => ({ 'data-title': attributes.title ?? 'Параграф' }),
            },
            ...titleColorAttribute(),
            kind: {
                default: 'paragraph',
                parseHTML: (element: HTMLElement) => element.getAttribute('data-kind') ?? 'paragraph',
                renderHTML: (attributes: { kind?: string }) => ({ 'data-kind': attributes.kind ?? 'paragraph' }),
            },
            icon: {
                default: 'pi pi-info-circle',
                parseHTML: (element: HTMLElement) => element.getAttribute('data-icon') ?? 'pi pi-info-circle',
                renderHTML: (attributes: { icon?: string }) => ({ 'data-icon': attributes.icon ?? 'pi pi-info-circle' }),
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
            borderLeftWidth: borderWidthAttribute('left', 1),
            borderRadius: {
                default: 6,
                parseHTML: (element: HTMLElement) => Number(element.getAttribute('data-border-radius') ?? 6),
                renderHTML: (attributes: { borderRadius?: number }) => ({ 'data-border-radius': String(attributes.borderRadius ?? 6) }),
            },
        }
    },
    parseHTML: () => [{ tag: 'section[data-recipe-block="groupBox"]' }],
    renderHTML: ({ node, HTMLAttributes }) => {
        const borderWidth = [node.attrs.borderTopWidth, node.attrs.borderRightWidth, node.attrs.borderBottomWidth, node.attrs.borderLeftWidth]
            .map((value) => `${Math.max(0, Number(value ?? 1))}px`)
            .join(' ')
        const style = [
            'border-style: solid',
            `border-color: ${node.attrs.borderColor || '#b7c8bb'}`,
            `border-width: ${borderWidth}`,
            `border-radius: ${Math.max(0, Number(node.attrs.borderRadius ?? 6))}px`,
            node.attrs.backgroundColor ? `background-color: ${node.attrs.backgroundColor}` : '',
        ].filter(Boolean).join('; ')
        return [
            'section',
            mergeAttributes(HTMLAttributes, { 'data-recipe-block': 'groupBox', style }),
            ['div', { class: 'group-box__heading', style: node.attrs.titleColor ? `color: ${node.attrs.titleColor}` : '' },
                ['i', { class: node.attrs.icon || 'pi pi-info-circle' }],
                ['strong', {}, node.attrs.title || 'Параграф']],
            ['div', { class: 'group-box__content' }, 0],
        ]
    },
    addNodeView: () => VueNodeViewRenderer(GroupBoxView as unknown as Component),
})
