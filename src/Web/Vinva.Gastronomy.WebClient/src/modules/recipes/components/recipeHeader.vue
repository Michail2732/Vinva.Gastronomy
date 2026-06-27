<template >
    <div>
        <div class="grid recipe-header-container">
            <div class="col-12 md:col-6 recipe-img-container recipe-header-col1-container">
                <Image src="/logo.svg" class="recipe-img" />
            </div>
            <div class="col-12 md:col-6 recipe-header-col2-container">
                <div class="categories-container">
                    <span v-for="property in vm?.properties" class="category-text">
                        {{ property.name }}
                        <Tag v-for="val in property.values" value="{{ val }}">
                            {{ val }}
                        </Tag>
                    </span>
                </div>                    
                <div class="recipe-name-container">
                    <span class="recipe-name-text">
                        {{ vm?.name }}
                    </span>
                </div>
                <div class="recipe-properties-container">
                    <div class="recipe-desc-container">
                        <span class="pi pi-receipt recipe-desc-header-icon"></span>
                        <span class="recipe-desc-header"> Описание</span>
                        <br/>
                        <span class="recipe-desc-content">{{ vm?.description ?? 'Нет описания' }}</span>
                        <Comment :title="'ОБЩИЙ КОММЕНТАРИЙ'"
                                 class="recipe-common-comment"
                                 :variant="'neutral'"
                                 :content="(vm?.comment || 'Нет общего коментария')"/>
                    </div>
                    <div class="recipe-cook-time-container">
                        <span class="cook-time-label">
                            ВРЕМЯ ПРИГОТОВЛЕНИЯ
                        </span>
                        <br/>
                        <span class="cook-time-value">
                            {{ timeToMinutes(vm?.cookingTime!) }} мин
                        </span>
                    </div>                    
                </div>
            </div>
        </div>
    </div>
</template>
<script setup lang="ts">
import { ref } from 'vue';
import {type RecipeHeaderViewModel} from '../types/recipeTypes'
import Comment from '@/modules/ui/components/comment.vue';

function timeToMinutes(timeString: string) {
    if (!timeString)
        return '';
  const [hours, minutes, seconds] = timeString.split(':');
  return parseInt(hours!) * 60 + parseInt(minutes!) + parseInt(seconds!) / 60;
}


const props = defineProps<{recipeHeader: RecipeHeaderViewModel | null | undefined}>();
const vm = ref(props.recipeHeader);

</script>
<style scoped lang="scss">    
    @use "../../../assets/variables.scss" as *;

    .recipe-header-container
    {
        padding: 2rem 0;
        overflow-wrap: break-word;        
        word-break: break-all; 
        .recipe-img-container
        {
            .recipe-img
            {
                width: 100%;
                height: auto;
                display: block;
            }
        }
        .recipe-header-col2-container
        {
            padding: 1rem;
            display: flex;            
            flex-direction: column;
            justify-content: center;
            .categories-container
            {
                display: flex;
                justify-content: left;
                .category-text
                {
                    &:first-child
                    {
                        margin: 0 .2rem 0 0;    
                    }                    
                    letter-spacing: .05rem;
                    font-weight: 600;
                    font-size: .9rem;
                    padding: .3rem .7rem;
                    background-color: $sea-green-200-color;
                    color: $sea-green-600-color;                
                    border-radius: .8rem;
                    margin: 0 .2rem 0 .2rem;
                }
            }
            .recipe-name-container
            {
                margin-top: 1rem;
                .recipe-name-text
                {
                    line-height: 2;
                    letter-spacing: -1.2px;
                    font-size: 2.5rem;
                    font-weight: 800;
                    font-family: 'Inter' 600;
                }
            }                
            .recipe-properties-container
            {                    
                background-color: $violet-200-color;
                border-radius: 1.5rem;
                padding: 2rem ;
                margin-top: 1rem;
                font-family: 'Inter';
                overflow-wrap: break-word;
                word-wrap: break-word;
                word-break: normal;                                    
                .recipe-desc-container
                {                                                                                 
                    font-family: 'Inter';
                    .recipe-common-comment
                    {
                        margin: 1rem 0;
                    }
                    .recipe-desc-header-icon
                    {
                        font-size: 1rem;                
                    }
                    .recipe-desc-header
                    {
                        line-height: 1.2;
                        letter-spacing: 0rem;
                        font-size: 1.2rem;
                        font-weight: 600;
                        color: $dark-600-color
                    }
                    .recipe-desc-content
                    {
                        line-height: 1.8;
                        letter-spacing: 0rem;
                        font-size: 1rem;
                        font-style: italic;
                        color: $dark-400-color;
                    }                    
                }
                .recipe-cook-time-container
                {                        
                    display: block;
                    .cook-time-label
                    {                        
                        letter-spacing: .05rem;
                        font-weight: 500;
                        font-size: .8rem;
                        color: $dark-400-color;
                    }
                    .cook-time-value
                    {
                        text-align: center;
                        color: $sea-green-800-color;                        
                        letter-spacing: 0px;
                        font-weight: 500;
                        font-size: 1.3rem;
                    }                    
                }                
            }
        }                             
    }
</style>