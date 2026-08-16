<template >
    <div class="flex flex-column gap-4" >
        <div variant="over" >     
            <label class="font-bold mb-2 block">Название</label>       
            <InputText v-model="name" fluid/>
        </div>        
        <div variant="over" >
            <label class="font-bold mb-2 block" for="comment">Коментарий</label>
            <Textarea id="comment" class="h-6rem" v-model="comment"  fluid />            
        </div>
        <div variant="over" >            
            <CheckBox inputId="requiredChbx" v-model="required" name="required" value="true"/>
            <label class="font-medium mb-2 ml-2" for="requiredChbx">Обязательный</label>
        </div>
        <div variant="over" >
            <label class="font-bold mb-2 block" for="quantiies">Колличество</label>
            <AutoComplete id="quantiies" 
                          @value-change=""
                          v-model="quantiies" 
                          :invalid="quantitiesInvalid"
                          @item-select="" fluid />            
        </div>        
        <div class="flex justify-content-center">
            <Button v-if="props.mode=='create'" @click="SaveChanges" label="Добавить" />
            <Button v-if="props.mode=='edit'" @click="SaveChanges" label="Сохранить"/>
        </div>
    </div>
</template>
<script setup lang="ts">
import {ref, computed} from "vue"
import type {RecipeIngredientViewModel} from '../types/recipeTypes'
import * as utils from "../utils/utils"
import { DtoState } from "@/api/gastronomy_generated/types.gen";

const name = ref('');
const nameInvalid = computed<boolean>(() => 
{
    return utils.isAlphaNumericWithSpaces(name.value);
});
const id = ref('')
const comment = ref('');
const required = ref(false);
const state = ref<DtoState>(DtoState.NEW);
const quantiies = ref('');
const quantitiesInvalid = computed<boolean>(() => 
{
    return utils.tryParseQuantitiesString(quantiies.value).success;    
})

const props = defineProps<{
    ingredient: RecipeIngredientViewModel | null,
    mode: 'create' | 'edit'
}>();

const emits = defineEmits<{
    submit: [ingredient: RecipeIngredientViewModel]    
}>();
if (props.ingredient)
{
    id.value = props.ingredient.ingredientId;
    name.value = props.ingredient.ingredientName!;
    comment.value= props.ingredient.comment!;
    required.value = props.ingredient.isRequired!;
    state.value = props.ingredient.state!;
    quantiies.value = utils.getQuantitiesAsString(props.ingredient);
}
    
const SaveChanges = () => {
    emits('submit', {
        ingredientId: id.value,
        ingredientName: name.value,
        quantities: utils.tryParseQuantitiesString(quantiies.value).result!,
        comment: comment.value,
        isRequired: required.value,
        state: state.value
    });
}

</script>
<style lang="scss">
    
</style>