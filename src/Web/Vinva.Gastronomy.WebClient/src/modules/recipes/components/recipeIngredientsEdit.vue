<template>
    <div class="ingredients col-12 md:col-8 flex flex-column gap-4">
        <label class="font-bold mb-2 block">Список ингредиентов</label>
        <Listbox class="ingredients-list"/>
        <div class="flex justify-left flex-wrap gap-2 mb-3">
            <Button icononly rounded  severity="success">
                <i class="pi block pi-plus w-1rem h-1rem"></i>
            </Button>
            <Button icononly rounded  severity="danger">
                <i class="pi pi-trash"></i>
            </Button>
            <Button icononly rounded  severity="secondary">
                <span class="pi pi-pencil"></span>
            </Button>
        </div>
    </div>
</template>
<script setup lang="ts">
</script>
<style lang="scss">
    .ingredients
    {                
        margin: 0 auto;        
    }
    .ingredients-list
    {
        height: 8rem;
    }
</style>