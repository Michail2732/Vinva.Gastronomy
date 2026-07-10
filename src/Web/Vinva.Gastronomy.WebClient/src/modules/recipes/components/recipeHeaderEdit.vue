<template>
    <div class="header col-12 md:col-8 flex flex-column gap-4">
        <div class="recipe-name">  
            <label class="font-bold mb-2 block">Название</label>
            <InputText v-model="name" placeholder="Введите название рецепта" fluid/>
        </div>               
        <div class="description">
            <label class="font-bold block mb-2">Описание</label>
            <Editor v-model="description"/>
        </div>           
        <div>
            <label class="font-bold mb-2 block">Коментарий</label>
            <Editor v-model="comment"/>
        </div>        
        <div variant="over" class="recipe-time">     
            <label class="font-bold mb-2 block">Время приготовления (мин)</label>       
            <InputNumber mode="decimal"  v-model="time" fluid/>
        </div>      
    </div>
</template>
<script setup lang="ts">
import { ref } from 'vue';

const name = ref('');
const description = ref('');
const comment = ref('')
const time = ref(0);


</script>
<style lang="scss">
    .header
    {                
        margin: 0 auto;
    }
</style>