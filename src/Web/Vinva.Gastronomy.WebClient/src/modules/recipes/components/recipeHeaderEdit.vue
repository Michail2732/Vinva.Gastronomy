<template>
    <div class="header col-12 md:col-8 flex flex-column gap-4">
        <div>  
            <label class="font-bold mb-2 block">Название</label>
            <InputText v-model="name" placeholder="Введите название рецепта" fluid/>
        </div>
        <div>
            <span class="font-bold block mb-2">Описание</span>
            <Editor v-model="description" class="h-7rem" />
        </div>           
        <div class="mt-6">
            <span class="font-bold mb-2 block">Коментарий</span>
            <Editor v-model="comment" class="h-7rem"/>
        </div>        
        <div  class="mt-6">     
            <span class="font-bold mb-2 block">Время приготовления (мин)</span>
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