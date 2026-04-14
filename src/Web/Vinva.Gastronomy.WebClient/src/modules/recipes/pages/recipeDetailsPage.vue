<template>
   <div class="recipe-details-page-root">
        <div class="recipe-details-container">
            <Toast />
            <div class="grid recipe-header-container">
                <div class="col-12 md:col-6 recipe-img-container recipe-header-col1-container">
                    <Image src="/logo.svg" class="recipe-img" />
                </div>
                <div class="col-12 md:col-6 recipe-header-col2-container">
                    <div class="categories-container">
                        <span v-for="category in categories" class="category-text">
                            {{ category.name }}
                        </span>
                    </div>                    
                    <div class="recipe-name-container">
                        <span class="recipe-name-text">
                            {{ recipe?.name }}
                        </span>
                    </div>
                    <div class="recipe-cook-time-container">
                        <div class="recipe-common-cook-time-container">
                            <span class="cook-time-label">
                                ОБЩЕЕ ВРЕМЯ
                            </span>
                            <br/>
                            <span class="cook-time-value">
                                {{ timeToMinutes(recipe?.cookingTime!) }} мин
                            </span>
                        </div>
                        <div class="recipe-active-time-container">
                            <span class="cook-time-label">
                                АКТИВНОЕ
                            </span>
                            <br/>
                            <span class="cook-time-value">
                                NaN мин
                            </span>
                        </div>
                        <div class="recipe-common-cook-time-container">
                            <span class="cook-time-label">
                                ОЖИДАНИЕ
                            </span>
                            <br/>
                            <span class="cook-time-value">
                                NaN мин
                            </span>
                        </div>                        
                    </div>
                </div>
            </div>
            <div class="grid">
                <div class="col-12 md:col-8">
                    <div class="recipe-desc-container">
                        <span class="pi pi-receipt recipe-desc-header-icon"></span>
                        <span class="recipe-desc-header"> Описание</span>
                        <br/>
                        <span class="recipe-desc-content">{{ recipe?.description ?? 'Нет описания' }}</span>
                        <Divider/>
                        <span class="recipe-comment-header">ОБЩИЙ КОММЕНТАРИЙ</span>
                        <br/>
                        <span class="recipe-comment-content">{{ recipe?.comment ?? 'Нет комментария'}}</span>
                    </div>
                    <Divider align="left" type="solid"
                            class="prepare-header-container">
                        <span class="prapare-header">Приготовление</span>
                    </Divider>
                    <div v-for="(item, index) in recipe?.steps" 
                        class="recipe-step-container">
                        <label class="recipe-step-seq-number">{{ item.seqNumber }}</label>
                        <div class="recipe-step-desc-container">
                            <span class="recipe-step-desc">{{ item.description }}</span>
                            <Divider/>
                            <span class="recipe-step-comment">{{ item.comment}}</span>
                        </div>
                    </div>                    
                </div>
                <div class="col-12 md:col-4">                    
                </div>
            </div>
            <div>

            </div>
            <div>

            </div>
        </div>
    </div>
</template>
<script setup lang="ts">
import { useRecipesStore } from '../stores/recipesStore';
import { useToast } from 'primevue';
import { watch, ref, computed } from 'vue';
import { useRoute } from 'vue-router';
import type { RecipeDetailsViewModel } from '../types/recipeTypes';

const recipe = ref<RecipeDetailsViewModel | null>(null);
const categories = computed(() => recipe?.value?.categories ?? []);
const recipeStore = useRecipesStore();
const route = useRoute();
const toasts = useToast();

function timeToMinutes(timeString: string) {
  const [hours, minutes, seconds] = timeString.split(':');
  return parseInt(hours!) * 60 + parseInt(minutes!) + parseInt(seconds!) / 60;
}

async function loadRecipe(id: string)
{
    try 
    {
        const result = await recipeStore.getRecipeById(id);
        if (!result.isSuccess)
            toasts.add({severity: 'error', summary: 'Ошибка', detail: result.error, life: 3500});
        else 
            recipe.value = result.data;
    } 
    catch (error)
    {
        toasts.add({severity: 'error', summary: 'Ошибка', detail: error, life: 3500});            
    }        
}

loadRecipe(route.params.id as string);

watch(
    () => route.params.id,
    (newId) => 
    {
        if (newId)    
        {
            loadRecipe(newId as string);
        }
    },
    {immediate: true}
);

</script>
<style lang="scss">
    $violet-100-color: #f8f9ff;
    $violet-200-color: #eef4ff;    

    $dark-600-color: #121c28;
    $dark-400-color: #3C4A42;
        
    $sea-green-800-color: #006c49;
    $sea-green-600-color: #3C6C54;
    $sea-green-400-color: #5e8f76;
    $sea-green-200-color: #B7EBCE;    
    $sea-green-50-color: #e7f8f2;

    $swamp-400-color: #66726e;

.recipe-details-page-root
{
    background-color: $violet-100-color;
    .recipe-details-container
    {     
        max-width: 1200px;
        margin: 0 auto;

        .recipe-header-container
        {
            padding: 2rem 0;
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
                padding: .7rem;
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
                            margin: .2rem .4rem .2rem 0;    
                        }
                        line-height: 16px;
                        letter-spacing: 1.2px;
                        font-weight: 600;
                        font-size: 1rem;
                        padding: .6rem 1.2rem;
                        background-color: $sea-green-200-color;
                        color: $sea-green-600-color;                
                        border-radius: 1rem;
                        margin: .2rem .4rem;
                    }
                }

                .recipe-name-container
                {
                    margin-top: 2rem;
                    .recipe-name-text
                    {
                        line-height: 60px;
                        letter-spacing: -1.2px;
                        font-size: 3rem;
                        font-weight: 800;
                        font-family: 'Inter' 600;
                    }
                }                

                .recipe-cook-time-container
                {                    
                    background-color: $violet-200-color;
                    border-radius: 1.5rem;
                    padding: 2rem ;
                    margin-top: 2rem;
                    font-family: 'Inter';
                    display: flex;
                    justify-content: left;                    
                    .recipe-active-time-container
                    {                        
                        border-left: 1px solid $swamp-400-color;
                        border-right: 1px solid $swamp-400-color;
                        border-radius: .1rem;
                        margin: 0 1rem;
                        padding: 0 1rem;
                    }
                    .cook-time-label
                    {
                        line-height: 15px;
                        letter-spacing: 1px;
                        font-weight: 500;
                        font-size: .8rem;
                        color: $dark-400-color;
                    }
                    .cook-time-value
                    {
                        text-align: center;
                        color: $sea-green-800-color;                        
                        line-height: 28px;
                        letter-spacing: 0px;
                        font-weight: 500;
                        font-size: 1.8rem;
                    }                    
                }
            }                             
        }

        .recipe-desc-container
        {
            background-color: $violet-200-color;
            border-radius: 1.5rem;
            padding: 2rem;            
            margin-top: 2rem;
            font-family: 'Inter';
            .recipe-desc-header-icon
            {
                font-size: 1rem;                
            }
            .recipe-desc-header
            {
                line-height: 1.8rem;
                letter-spacing: 0rem;
                font-size: 1.2rem;
                font-weight: 600;
                color: $dark-600-color
            }
            .recipe-desc-content
            {
                line-height: 1.8rem;
                letter-spacing: 0rem;
                font-size: 1rem;
                font-style: italic;
                color: $dark-400-color;
            }                    
            .recipe-comment-header
            {
                line-height: 1rem;
                letter-spacing: -.01rem;
                font-size: .8rem;
                font-weight: 500;
                color: $dark-400-color;
            }
            .recipe-comment-content
            {
                line-height: 1.4rem;
                letter-spacing: 0rem;
                font-size: .9rem;
                color: $dark-600-color;
            }
        }


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
            .recipe-step-seq-number
            {
                width: 3rem;
                height: 3rem;
                border-radius: 1.5rem;
                background-color: $sea-green-800-color;                
            }    

            .recipe-step-desc-container
            {
                display: inline-block;
                .recipe-step-desc
                {
                
                }
                .recipe-step-comment
                {
                
                }
            }            
        }
    }
}    
</style>