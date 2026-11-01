import { Editor } from '@tiptap/vue-3'
import StarterKit from '@tiptap/starter-kit'
import Placeholder from '@tiptap/extension-placeholder'
import Image from '@tiptap/extension-image'
import Underline from '@tiptap/extension-underline'
import Color from '@tiptap/extension-color'
import TextAlign from '@tiptap/extension-text-align'
import type { JSONContent } from '@tiptap/core'
import { TextSelection } from '@tiptap/pm/state'
import {
    RecipeMedia, RecipeToggle, IngredientsSet, GroupBox, TextBlock,
    RecipeDivider, ImageGallery, RecipeImage, RecipeTaskList, RecipeTaskItem,
    RecipeTable, RecipeTableRow, RecipeTableHeader, RecipeTableCell, RecipeTextStyle, TableSelectionHighlight, RecipeTableColumnResize,
    RecipeBlockDrag, RecipeBlockMargins, RecipeCharacteristic,
} from '../recipeBlockExtensions'

export function createRecipeEditor(options: {
    content: JSONContent
    onUpdate: (editor: Editor) => void
    onSelectionUpdate: (editor: Editor) => void
}): Editor {
    return new Editor({
        extensions: [
            StarterKit.configure({ horizontalRule: false, blockquote: false, underline: false, trailingNode: false }),
            Placeholder.configure({ placeholder: 'Начните писать или добавьте блок из панели слева...' }),
            Image,
            Underline,
            RecipeTextStyle,
            Color,
            TextAlign.configure({
                types: ['paragraph', 'heading', 'groupBox', 'textBlock'],
                alignments: ['left', 'center', 'right'],
            }),
            RecipeTaskList,
            RecipeTaskItem,
            RecipeTable,
            TableSelectionHighlight,
            RecipeTableColumnResize,
            RecipeBlockDrag,
            RecipeTableRow,
            RecipeTableHeader,
            RecipeTableCell,
            RecipeToggle,
            RecipeMedia,
            IngredientsSet,
            RecipeCharacteristic,
            GroupBox,
            TextBlock,
            RecipeDivider,
            RecipeImage,
            ImageGallery,
            RecipeBlockMargins,
        ],
        content: options.content,
        onUpdate: ({ editor }) => options.onUpdate(editor as Editor),
        onSelectionUpdate: ({ editor }) => options.onSelectionUpdate(editor as Editor),
    })
}

export function rememberTextSelection(editor: Editor): { from: number; to: number } | null {
    const { from, to, empty } = editor.state.selection
    if (empty || !(editor.state.selection instanceof TextSelection)) return null
    return { from, to }
}
