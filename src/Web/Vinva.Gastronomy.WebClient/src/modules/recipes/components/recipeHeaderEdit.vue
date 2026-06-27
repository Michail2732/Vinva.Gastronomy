<template>
    <div class="header col-12 md:col-6 flex flex-column gap-4">
        <FloatLabel variant="over" class="recipe-name">            
            <InputText id="recipe-name-input"
                       v-model="name" fluid/>        
            <label for="recipe-name-input">
                Название
            </label>                        
        </FloatLabel>      
        <div class="description">
            <label>Описание</label>
            <Editor id v-model="description"/>
        </div>           
        <div>
            <label>Коментарий</label>
            <Editor v-model="comment"/>
        </div>        
        <FloatLabel variant="over" class="recipe-time">            
            <InputText id="recipe-time-input"
                       v-model="name" fluid/>        
            <label for="recipe-time-input">
                Время приготовления
            </label>                        
        </FloatLabel>      
    </div>
</template>
<script setup lang="ts">
import { ref } from 'vue';

const name = ref('');
const description = ref('');
const comment = ref('')


</script>
<style lang="scss">
    .header
    {
        max-width: 1200px;
        margin: 0 auto;
    }
</style>