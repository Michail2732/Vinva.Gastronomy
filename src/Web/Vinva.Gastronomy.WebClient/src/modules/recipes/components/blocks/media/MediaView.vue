<template>
    <NodeViewWrapper class="recipe-block recipe-media" :class="[`recipe-media--${node.attrs.kind}`, { 'recipe-media--bookmark-view': isMinimalBookmarkView }]" :style="spacingStyle">
        <BlockDragHandle v-if="isEditable" :editor="editor" />
        <RecipeNodeToolbar :editable="isEditable" :delete-node="deleteNode" delete-label="Удалить медиа">
            <RecipeBlockSpacing :attrs="node.attrs" :update-attributes="updateAttributes" />
        </RecipeNodeToolbar>
        <div v-if="!isMinimalBookmarkView" class="recipe-media__heading">
            <i :class="kindIcon" />
            <span>{{ kindLabel }}</span>
        </div>
        <input v-if="isEditable" class="p-inputtext p-component recipe-media__url" type="url" :value="node.attrs.url" :placeholder="urlPlaceholder" aria-label="Ссылка на медиа" @input="update('url', $event)" />
        <iframe v-if="node.attrs.kind === 'embed' && embedUrl" class="recipe-media__embed" :src="embedUrl" title="Встроенный плеер" loading="lazy" referrerpolicy="strict-origin-when-cross-origin" sandbox="allow-scripts allow-same-origin allow-presentation" allow="accelerometer; autoplay; encrypted-media; gyroscope; picture-in-picture" allowfullscreen />
        <video v-if="node.attrs.kind === 'video' && safeUrl" class="recipe-media__video" :src="safeUrl" controls preload="metadata" />
        <audio v-else-if="node.attrs.kind === 'audio' && safeUrl" class="recipe-media__audio" :src="safeUrl" controls preload="metadata" />
        <a v-else-if="['bookmark', 'file'].includes(node.attrs.kind) && safeUrl" class="recipe-media__bookmark" :href="safeUrl" target="_blank" rel="noopener noreferrer">
            <i :class="linkIcon" />
            <span>{{ node.attrs.caption || safeUrl }}</span>
        </a>
        <p v-if="node.attrs.url && !safeUrl" class="recipe-media__hint">Укажите ссылку, начинающуюся с http:// или https://</p>
        <p v-else-if="node.attrs.kind === 'embed' && safeUrl && !embedUrl" class="recipe-media__hint">Поддерживаются ссылки YouTube и Vimeo</p>
        <input v-if="isEditable" class="p-inputtext p-component recipe-media__caption" :value="node.attrs.caption" placeholder="Подпись (необязательно)" aria-label="Подпись к медиа" @input="update('caption', $event)" />
        <p v-else-if="node.attrs.caption && !isMinimalBookmarkView" class="recipe-media__caption-view">{{ node.attrs.caption }}</p>
    </NodeViewWrapper>
</template>

<script setup lang="ts">
import { computed, inject, ref } from 'vue'
import type { Ref } from 'vue'
import type { Editor } from '@tiptap/core'
const isEditable = inject<Ref<boolean>>('editorEditable', ref(true))
import { NodeViewWrapper } from '@tiptap/vue-3'
import RecipeNodeToolbar from '../../editor/recipeNodeToolbar.vue'
import RecipeBlockSpacing from '../../editor/recipeBlockSpacing.vue'
import { blockMarginStyle } from '../../shared/margins'
import BlockDragHandle from '../drag/BlockDragHandle.vue'

const props = defineProps<{
    editor: Editor
    node: { attrs: { kind: 'video' | 'audio' | 'bookmark' | 'embed' | 'file'; url: string; caption: string } }
    updateAttributes: (attrs: Record<string, unknown>) => void
    deleteNode: () => void
}>()

const spacingStyle = computed(() => blockMarginStyle(props.node.attrs as unknown as Record<string, unknown>))
const isMinimalBookmarkView = computed(() => !isEditable.value && props.node.attrs.kind === 'bookmark')

const kindIcon = computed(() => ({ video: 'pi pi-video', audio: 'pi pi-volume-up', bookmark: 'pi pi-link', embed: 'pi pi-window-maximize', file: 'pi pi-paperclip' })[props.node.attrs.kind])
const linkIcon = computed(() => props.node.attrs.kind === 'bookmark' ? 'pi pi-link' : 'pi pi-external-link')
const kindLabel = computed(() => ({ video: 'Видео', audio: 'Аудио', bookmark: 'Ссылка', embed: 'Встроенный плеер', file: 'Файл' })[props.node.attrs.kind])
const urlPlaceholder = computed(() => props.node.attrs.kind === 'bookmark' ? 'https://...' : 'https://...')
const safeUrl = computed(() => {
    try {
        const url = new URL(props.node.attrs.url)
        return url.protocol === 'http:' || url.protocol === 'https:' ? url.href : ''
    } catch {
        return ''
    }
})
const embedUrl = computed(() => {
    if (props.node.attrs.kind !== 'embed' || !safeUrl.value) return ''
    const url = new URL(safeUrl.value)
    const host = url.hostname.toLowerCase()
    if (host === 'youtu.be') {
        const videoId = url.pathname.slice(1)
        return /^[\w-]{11}$/.test(videoId) ? `https://www.youtube-nocookie.com/embed/${videoId}` : ''
    }
    if (host === 'youtube.com' || host === 'www.youtube.com' || host === 'm.youtube.com') {
        const videoId = url.searchParams.get('v')
        return videoId && /^[\w-]{11}$/.test(videoId) ? `https://www.youtube-nocookie.com/embed/${videoId}` : ''
    }
    if (host === 'vimeo.com' || host === 'www.vimeo.com') {
        const videoId = url.pathname.split('/').filter(Boolean)[0]
        return videoId && /^\d+$/.test(videoId) ? `https://player.vimeo.com/video/${videoId}` : ''
    }
    return ''
})

function update(field: 'url' | 'caption', event: Event) {
    props.updateAttributes({ [field]: (event.target as HTMLInputElement).value })
}
</script>

<style scoped lang="scss">
.recipe-media { background: var(--p-surface-50); border: 1px solid var(--p-content-border-color); border-radius: var(--p-border-radius-md, .5rem); display: grid; gap: .65rem; margin: 1rem 0; padding: 1rem; }
.recipe-media.recipe-media--bookmark-view { background: transparent; border: 0; padding: 0; }
.recipe-media--bookmark-view .recipe-media__bookmark { background: transparent; border: 0; padding: 0; }
.recipe-media__heading { align-items: center; color: var(--p-text-color); display: flex; font-size: .8rem; font-weight: 700; gap: .5rem; }
.recipe-media__heading i { color: var(--p-primary-color); }
.recipe-media__url, .recipe-media__caption { width: 100%; }
.recipe-media__video { background: var(--p-surface-900); border-radius: var(--p-border-radius-md, .5rem); max-height: 420px; width: 100%; }
.recipe-media__embed { aspect-ratio: 16 / 9; border: 0; border-radius: var(--p-border-radius-md, .5rem); width: 100%; }
.recipe-media__audio { width: 100%; }
.recipe-media__bookmark { align-items: center; background: var(--p-surface-0); border: 1px solid var(--p-content-border-color); border-radius: 4px; color: var(--p-primary-700); display: flex; gap: .6rem; min-width: 0; overflow-wrap: anywhere; padding: .8rem; text-decoration: none; }
.recipe-media__hint { color: var(--p-orange-700); font-size: .75rem; margin: 0; }
.recipe-media__caption-view { color: var(--p-text-muted-color); font-size: .78rem; margin: 0; }
</style>
