import { Node, mergeAttributes } from '@tiptap/core'
import type { Component } from 'vue'
import { VueNodeViewRenderer } from '@tiptap/vue-3'
import DividerView from './DividerView.vue'

export const RecipeDivider = Node.create({
    name: 'horizontalRule',
    group: 'block',
    atom: true,
    parseHTML: () => [{ tag: 'div[data-recipe-block="horizontalRule"]' }],
    renderHTML: ({ HTMLAttributes }) => ['div', mergeAttributes(HTMLAttributes, { 'data-recipe-block': 'horizontalRule' }), ['hr']],
    addNodeView: () => VueNodeViewRenderer(DividerView as unknown as Component),
})
