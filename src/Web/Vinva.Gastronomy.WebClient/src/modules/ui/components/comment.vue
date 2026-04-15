<template>
    <div class="comment-container">
        <label class="comment-header">
            <slot name="icon">
                <i class="pi pi-lightbulb comment-header-icon"></i>
            </slot>            
            <span class="comment-header-text">{{ title }}</span>
        </label>
        <br/>
        <span class="comment-text">{{ content }}</span>
    </div>
</template>
<script setup lang="ts">
const props = defineProps<{title: string | null | undefined, content: string | null | undefined}>();
</script>
<style scoped lang="scss">
@use "../../../assets/variables.scss" as *;

    .comment-container
    {
        font-family: 'Inter';
        color: $sea-green-800-color;
        border-left: 4px solid $sea-green-800-color;
        padding: 1rem;        
        border-radius: 1rem;
        background-color: $sea-green-50-color;        
        .comment-header
        {                
            .comment-header-icon
            {
                padding: 0;
                margin: 0 .3rem 0 0;
                display: inline-block;
                font-size: .8rem;                
            }
            
            .comment-header-text
            {
                line-height: .9rem;
                letter-spacing: .05rem;
                font-size: .7rem;
                font-weight: 600;
            }
        }
        .comment-text
        {
            display: block;
            margin-top: .5rem;
            line-height: 1.1rem;
            letter-spacing: 0rem;
            font-size: .8rem;
            font-weight: 300;            
        }
    }
</style>