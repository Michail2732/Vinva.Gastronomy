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
                <Divider class="recipe-step-desc-devider"/>
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
const steps = props.steps;

</script>
<style scoped lang="scss">  
@use "../../../assets/variables.scss" as *;

   .prepare-header-container
    {
        margin-top: 3rem;
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
        .recipe-step-seq-number
        {
            width: 3rem;
            height: 3rem;
            border-radius: 1.5rem;
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
                line-height: 1.6rem;
                letter-spacing: 0rem;
                font-size: 1rem;
            }
            .recipe-step-comment
            {
                line-height: 1.6rem;
                letter-spacing: 0rem;
                font-size: 1rem;
                font-style: italic;
            }
        }            
    }
</style>