<template >
    <div>
        <Divider align="left" type="solid"
                class="prepare-header-container">
            <span class="prapare-header">Приготовление</span>
        </Divider>
        <div v-for="(item, index) in steps" 
            class="recipe-step-container">
            <label class="recipe-step-seq-number">
                <span>{{ item.seqNumber }}</span>                        
            </label>
            <div class="recipe-step-desc-container">
                <span class="recipe-step-desc">{{ item.description}}</span>
                <br/>                
                <span class="recipe-step-comment">{{ item.comment}}</span>
            </div>
        </div>                    
    </div>
</template>
<script setup lang="ts">
import {ref} from 'vue'
import type { RecipeStepDto } from '@/api/gastronomy_generated';

const props = withDefaults(
    defineProps<{steps: RecipeStepDto[] | null | undefined}>(),
    {
        steps: () => []
    }
) 
const steps = ref(props.steps);

</script>
<style scoped lang="scss">  
@use "../../../assets/variables.scss" as *;

   .prepare-header-container
    {        
        :deep(.p-divider-content)
        {
            background-color: $violet-100-color;
        }
        .prapare-header
        {
            background-color: $violet-100-color;
            line-height: 2.1rem;
            letter-spacing: -.01rem;
            font-size: 1.8rem;
            font-weight: 800;
        }
    }

    .recipe-step-container
    {
        display: flex;
        margin: 0 0 2rem 0;     
        overflow-wrap: break-word;        
        word-break: break-all;        
        .recipe-step-seq-number
        {
            width: 2.5rem;
            height: 2.5rem;
            border-radius: 1.25rem;
            background-color: $sea-green-800-color;        
            margin: 0 1rem 0 0;                
            position: relative;    
            span 
            {
                color: white;
                line-height: 1.7rem;
                letter-spacing: 0rem;
                font-size: 1.1rem;
                font-weight: 500;
                position: absolute;
                top: 50%;
                left: 50%;
                transform: translate(-50%, -50%);
            }
        }    
        .recipe-step-desc-container
        {
            display: inline-block;                
            color: $dark-600-color;
            .recipe-step-desc-devider
            {
                margin: .5rem 0;            
            }
            .recipe-step-desc
            {
                line-height: 1.5;
                letter-spacing: 0rem;
                font-size: 1rem;
            }
            .recipe-step-comment
            {
                color: $dark-400-color;
                line-height: 1.6;
                letter-spacing: 0rem;
                font-size: 1rem;
                font-style: italic;
            }
        }            
    }
</style>