import TaskList from '@tiptap/extension-task-list'
import TaskItem from '@tiptap/extension-task-item'
import { VueNodeViewRenderer } from '@tiptap/vue-3'
import type { Component } from 'vue'
import { TaskListView } from '../taskListView'

export const RecipeTaskList = TaskList.extend({
    addNodeView() {
        return VueNodeViewRenderer(TaskListView as unknown as Component)
    },
})
export const RecipeTaskItem = TaskItem.configure({ nested: true })
