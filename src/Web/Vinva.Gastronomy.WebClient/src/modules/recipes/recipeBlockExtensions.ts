import { Extension, Node, mergeAttributes } from '@tiptap/core'
import { VueNodeViewRenderer } from '@tiptap/vue-3'
import TaskList from '@tiptap/extension-task-list'
import TaskItem from '@tiptap/extension-task-item'
import { Table, TableCell, TableHeader, TableRow } from '@tiptap/extension-table'
import Color from '@tiptap/extension-color'
import { TextStyle } from '@tiptap/extension-text-style'
import { NodeSelection, Plugin } from '@tiptap/pm/state'
import { CellSelection } from '@tiptap/pm/tables'
import { Decoration, DecorationSet, type EditorView } from '@tiptap/pm/view'
import RecipeIngredientView from './components/editor/recipeIngredient.vue'
import RecipeParagraphView from './components/editor/recipeParagraph.vue'
import RecipeGalleryView from './components/editor/recipeGalery.vue'
import RecipeImageView from './components/editor/recipeImage.vue'
import RecipeDividerView from './components/editor/recipeDivider.vue'
import RecipeSectionView from './components/editor/recipeSection.vue'
import RecipeMediaView from './components/editor/recipeMedia.vue'

type NodeViewComponent = Parameters<typeof VueNodeViewRenderer>[0]

function getCellPosition(view: EditorView, target: EventTarget | null) {
    if (!(target instanceof Element)) return null
    const cell = target.closest('td, th')
    if (!cell || !view.dom.contains(cell)) return null

    const position = view.posAtDOM(cell, 0)
    const resolved = view.state.doc.resolve(position)
    for (let depth = resolved.depth; depth > 0; depth--) {
        const node = resolved.node(depth)
        if (node.type.name === 'tableCell' || node.type.name === 'tableHeader') return resolved.before(depth)
    }
    return null
}

export const TableSelectionHighlight = Extension.create({
    name: 'tableSelectionHighlight',
    addProseMirrorPlugins() {
        let stopCellDrag = () => {}
        return [new Plugin({
            props: {
                handleDOMEvents: {
                    mousedown: (view, event) => {
                        if (event.button !== 0) return false
                        const anchorPosition = getCellPosition(view, event.target)
                        const anchorCell = event.target instanceof Element ? event.target.closest('td, th') : null
                        const anchorTable = anchorCell?.closest('table')
                        if (anchorPosition === null || !anchorTable) return false

                        stopCellDrag()
                        const document = view.dom.ownerDocument
                        const startX = event.clientX
                        const startY = event.clientY
                        const endDrag = () => {
                            document.removeEventListener('mousemove', onMouseMove, true)
                            document.removeEventListener('mouseup', endDrag, true)
                            stopCellDrag = () => {}
                        }
                        const onMouseMove = (moveEvent: MouseEvent) => {
                            if (!(moveEvent.buttons & 1)) {
                                endDrag()
                                return
                            }
                            if (Math.hypot(moveEvent.clientX - startX, moveEvent.clientY - startY) < 4) return

                            const headCell = document.elementFromPoint(moveEvent.clientX, moveEvent.clientY)?.closest('td, th')
                            if (!headCell || headCell.closest('table') !== anchorTable) return
                            const headPosition = getCellPosition(view, headCell)
                            if (headPosition === null) return

                            const selection = CellSelection.create(view.state.doc, anchorPosition, headPosition)
                            if (!selection.eq(view.state.selection)) view.dispatch(view.state.tr.setSelection(selection))
                            moveEvent.preventDefault()
                        }

                        document.addEventListener('mousemove', onMouseMove, true)
                        document.addEventListener('mouseup', endDrag, true)
                        stopCellDrag = endDrag
                        return false
                    },
                },
                decorations: (state) => {
                    const decorations: Decoration[] = []
                    const { selection } = state
                    const rootPosition = selection.$from.depth > 0 ? selection.$from.before(1) : selection instanceof NodeSelection ? selection.from : null
                    if (rootPosition !== null) {
                        const rootNode = state.doc.nodeAt(rootPosition)
                        if (rootNode?.isBlock)
                            decorations.push(Decoration.node(rootPosition, rootPosition + rootNode.nodeSize, { class: 'is-active-block' }))
                    }

                    for (let depth = selection.$from.depth; depth > 0; depth--) {
                        const node = selection.$from.node(depth)
                        const position = selection.$from.before(depth)
                        if (node.type.name === 'table')
                            decorations.push(Decoration.node(position, position + node.nodeSize, { class: 'is-active-table' }))
                        if (node.type.name === 'tableCell' || node.type.name === 'tableHeader')
                            decorations.push(Decoration.node(position, position + node.nodeSize, { class: 'is-active-cell' }))
                    }
                    return DecorationSet.create(state.doc, decorations)
                },
            },
            view: () => ({ destroy: () => stopCellDrag() }),
        })]
    },
})

function asNodeViewComponent(component: unknown) {
    return component as NodeViewComponent
}

function titleColorAttribute() {
    return {
        titleColor: {
            default: null,
            parseHTML: (element: HTMLElement) => element.getAttribute('data-title-color'),
            renderHTML: (attributes: { titleColor?: string | null }) => attributes.titleColor ? { 'data-title-color': attributes.titleColor } : {},
        },
    }
}

function createTextBlock(name: string, view: unknown, defaultTitle?: string) {
    return Node.create({
        name,
        group: 'block',
        content: 'inline*',
        defining: true,
        addAttributes() {
            if (!defaultTitle) return {}
            return {
                ...titleColorAttribute(),
                title: {
                    default: defaultTitle,
                    parseHTML: (element: HTMLElement) => element.getAttribute('data-title') ?? defaultTitle,
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
            }
        },
        parseHTML: () => [{ tag: `div[data-recipe-block="${name}"]` }],
        renderHTML: ({ HTMLAttributes }) => [
            'div',
            mergeAttributes(HTMLAttributes, { 'data-recipe-block': name }),
            ['p', 0],
        ],
        addNodeView: () => VueNodeViewRenderer(asNodeViewComponent(view)),
    })
}

export const RecipeParagraph = Node.create({
    name: 'recipeParagraph',
    group: 'block',
    content: 'inline*',
    defining: true,
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
            borderTopWidth: borderWidthAttribute('top'),
            borderRightWidth: borderWidthAttribute('right'),
            borderBottomWidth: borderWidthAttribute('bottom'),
            borderLeftWidth: borderWidthAttribute('left'),
            borderRadius: {
                default: 6,
                parseHTML: (element: HTMLElement) => Number(element.getAttribute('data-border-radius') ?? 6),
                renderHTML: (attributes: { borderRadius?: number }) => ({ 'data-border-radius': String(attributes.borderRadius ?? 6) }),
            },
        }
    },
    parseHTML: () => [{ tag: 'section[data-recipe-block="recipeParagraph"]' }],
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
            mergeAttributes(HTMLAttributes, { 'data-recipe-block': 'recipeParagraph', style }),
            ['div', { class: 'recipe-paragraph__heading', style: node.attrs.titleColor ? `color: ${node.attrs.titleColor}` : '' },
                ['i', { class: node.attrs.icon || 'pi pi-info-circle' }],
                ['strong', {}, node.attrs.title || 'Параграф']],
            ['p', { class: 'recipe-paragraph__content' }, 0],
        ]
    },
    addNodeView: () => VueNodeViewRenderer(asNodeViewComponent(RecipeParagraphView)),
})

function borderWidthAttribute(side: 'top' | 'right' | 'bottom' | 'left') {
    const field = `border${side[0]!.toUpperCase()}${side.slice(1)}Width`
    return {
        default: 1,
        parseHTML: (element: HTMLElement) => Number(element.getAttribute(`data-border-${side}-width`) ?? 1),
        renderHTML: (attributes: Record<string, number>) => ({ [`data-border-${side}-width`]: String(attributes[field] ?? 1) }),
    }
}

export const RecipeToggle = createTextBlock('recipeToggle', RecipeSectionView)

export const RecipeTaskList = TaskList
export const RecipeTaskItem = TaskItem.configure({ nested: true })
export const RecipeTextStyle = TextStyle.extend({
    addAttributes() {
        return {
            ...this.parent?.(),
            fontSize: {
                default: null,
                parseHTML: (element: HTMLElement) => element.style.fontSize || null,
                renderHTML: (attributes: { fontSize?: string | null }) => attributes.fontSize ? { style: `font-size: ${attributes.fontSize}` } : {},
            },
            fontFamily: {
                default: null,
                parseHTML: (element: HTMLElement) => element.style.fontFamily || null,
                renderHTML: (attributes: { fontFamily?: string | null }) => attributes.fontFamily ? { style: `font-family: "${attributes.fontFamily}"` } : {},
            },
        }
    },
})
export const RecipeTextColor = Color

function tableStyleAttributes() {
    return {
        backgroundColor: {
            default: null,
            parseHTML: (element: HTMLElement) => element.style.getPropertyValue('--recipe-table-background') || null,
            renderHTML: (attributes: { backgroundColor?: string | null }) => attributes.backgroundColor ? { style: `--recipe-table-background: ${attributes.backgroundColor};` } : {},
        },
        gridColor: {
            default: '#d1d5db',
            parseHTML: (element: HTMLElement) => element.style.getPropertyValue('--recipe-grid-color') || '#d1d5db',
            renderHTML: (attributes: { gridColor?: string }) => ({ style: `--recipe-grid-color: ${attributes.gridColor || '#d1d5db'};` }),
        },
        gridWidth: {
            default: 1,
            parseHTML: (element: HTMLElement) => Number.parseFloat(element.style.getPropertyValue('--recipe-grid-width')) || 1,
            renderHTML: (attributes: { gridWidth?: number }) => ({ style: `--recipe-grid-width: ${Math.min(20, Math.max(0, Number(attributes.gridWidth) || 0))}px;` }),
        },
    }
}

function cellBackgroundAttribute() {
    return {
        backgroundColor: {
            default: null,
            parseHTML: (element: HTMLElement) => element.style.backgroundColor || null,
            renderHTML: (attributes: { backgroundColor?: string | null }) => attributes.backgroundColor ? { style: `background-color: ${attributes.backgroundColor};` } : {},
        },
    }
}

export const RecipeTable = Table.extend({
    addAttributes() {
        return { ...this.parent?.(), ...tableStyleAttributes() }
    },
}).configure({ resizable: true })
export const RecipeTableRow = TableRow
export const RecipeTableHeader = TableHeader.extend({
    addAttributes() {
        return { ...this.parent?.(), ...cellBackgroundAttribute() }
    },
})
export const RecipeTableCell = TableCell.extend({
    addAttributes() {
        return { ...this.parent?.(), ...cellBackgroundAttribute() }
    },
})

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
        ['p', {}, [node.attrs.caption || node.attrs.url || 'Добавьте ссылку']],
    ],
    addNodeView: () => VueNodeViewRenderer(asNodeViewComponent(RecipeMediaView)),
})

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
    addNodeView: () => VueNodeViewRenderer(asNodeViewComponent(RecipeIngredientView)),
})

export const RecipeDivider = Node.create({
    name: 'horizontalRule',
    group: 'block',
    atom: true,
    parseHTML: () => [{ tag: 'div[data-recipe-block="horizontalRule"]' }],
    renderHTML: ({ HTMLAttributes }) => ['div', mergeAttributes(HTMLAttributes, { 'data-recipe-block': 'horizontalRule' }), ['hr']],
    addNodeView: () => VueNodeViewRenderer(asNodeViewComponent(RecipeDividerView)),
})

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
        ['div', { class: 'gallery-grid' }, ...(HTMLAttributes.images as string[]).map((src) => ['img', { src, alt: 'Кадр процесса' }])],
    ],
    addNodeView: () => VueNodeViewRenderer(asNodeViewComponent(RecipeGalleryView)),
})

export const RecipeImage = Node.create({
    name: 'recipeImage',
    group: 'block',
    atom: true,
    draggable: true,
    addAttributes: () => ({
        src: { default: '/logo.svg' },
        alt: { default: 'Изображение рецепта' },
        caption: { default: 'Добавьте подпись к изображению' },
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
    addNodeView: () => VueNodeViewRenderer(asNodeViewComponent(RecipeImageView)),
})
