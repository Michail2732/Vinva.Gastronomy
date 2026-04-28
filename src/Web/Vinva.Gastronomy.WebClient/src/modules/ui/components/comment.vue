<template>
    <div class="comment-container"
         :class="containerClassObject">
        <div class="comment-header">
            <slot name="icon"><i class="pi pi-lightbulb comment-header-icon"></i></slot>            
            <span class="comment-header-text">{{ title }}</span>
        </div>        
        <span class="comment-text">{{ content }}</span>
    </div>
</template>
<script setup lang="ts">

import {computed, ref} from 'vue'

type CommentVariant = 'primary' | 'neutral'

const props = defineProps<
{
    title: string | null | undefined,
    content: string | null | undefined,
    variant?: CommentVariant
}>();

const containerClassObject = computed(() => {
    if (!props.variant || props.variant === 'primary') {
        return { 'comment-container--primary': true };
    }
        
    if (props.variant === 'neutral') {
        return { 'comment-container--alternate': true };
    }
        
    return { 'comment-container--primary': true };
});

</script>
<style scoped lang="scss">
@use "../../../assets/variables.scss" as *;

    .comment-container
    {
        font-family: 'Inter';
        padding: 1.5rem;
        border-radius: 1rem;        
        font-size: 2rem;
        overflow-wrap: break-word;
        word-wrap: break-word;
        word-break: normal;
        .comment-header
        {                 
            line-height: 0.6;   
            .comment-header-icon
            {                
                margin: 0 .3em 0 0;
                font-size: .4em;
                font-weight: 600;
            }
            
            .comment-header-text
            {                
                letter-spacing: .05em;                
                font-size: .4em;
                font-weight: 600;
            }
        }
        .comment-text
        {
            display: block;
            line-height: 1.5;
            margin-top: .7em;        
            letter-spacing: 0em;
            font-size: .42em;
            font-weight: 300;            
        }
    }

    .comment-container--primary
    {
        color: $sea-green-800-color;
        border-left: 4px solid $sea-green-800-color;
        background-color: $sea-green-50-color;        
    }

    .comment-container--alternate
    {
        background-color: #f8f9fa;
        border-left: 4px solid #6c757d;
        color: #495057;
    }
</style>