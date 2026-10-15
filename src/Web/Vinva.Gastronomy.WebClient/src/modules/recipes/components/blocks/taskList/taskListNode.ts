import TaskList from '@tiptap/extension-task-list'
import TaskItem from '@tiptap/extension-task-item'

export const RecipeTaskList = TaskList
export const RecipeTaskItem = TaskItem.configure({ nested: true })
