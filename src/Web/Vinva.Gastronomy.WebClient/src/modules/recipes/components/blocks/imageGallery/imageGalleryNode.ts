import { Node, mergeAttributes } from '@tiptap/core'
import type { Component } from 'vue'
import { VueNodeViewRenderer } from '@tiptap/vue-3'
import GalleryView from './GalleryView.vue'

export const ImageGallery = Node.create({
    name: 'imageGallery',
    group: 'block',
    atom: true,
    draggable: true,
    addAttributes: () => ({ images: { default: ['/logo.svg', '/logo.svg', '/logo.svg'] } }),
    parseHTML: () => [{ tag: 'div[data-recipe-block="imageGallery"]' }],
    renderHTML: ({ HTMLAttributes }) => [
        'div',
        mergeAttributes(HTMLAttributes, { 'data-recipe-block': 'imageGallery' }),
        ['div', { class: 'gallery-grid' }, ...(HTMLAttributes.images as string[]).map((src) => ['img', { src, alt: '' }])],
    ],
    addNodeView: () => VueNodeViewRenderer(GalleryView as unknown as Component),
})
