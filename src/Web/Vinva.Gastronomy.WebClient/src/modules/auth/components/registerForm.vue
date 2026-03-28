<template >
    <div class="login-form">
        <FloatLabel variant="over" class="input-part">
            <InputText id="email_lbl" @value-change="emailValidate  " 
                       v-model="email" 
                :invalid="!isEmailValid" fluid />
            <label class="label-head" for="email_lbl">Введите email</label>
            <Message v-if="!emailError" severity="error">{{ emailError }}</Message>
        </FloatLabel>        
        <Button label="Войти" @click="handleSubmit" class="submit-login" />
    </div>
</template>
<script setup lang="ts">
import {ref} from 'vue'


const email = ref('');
const isEmailValid = ref(true);
const emailError = ref('');

const emailValidate = () => 
{
    const emailRegex =/^[^\s@]+@([^\s@]+\.)+[^\s@]+$/
    isEmailValid.value = !!email.value || emailRegex.test(email.value);    
    emailError.value = isEmailValid.value ? '' : 'Некорректный email';        
    return isEmailValid;
};    

const props = defineProps({
        registerErrors: 
        {
            type: String,
            required: false,
            default: ''
        },
        isRegisterInProgress:
        {
            type: Boolean,
            required: false,
            default: false
        }
    });
const emits = defineEmits<{
    submit: [email: string]
}>();

const handleSubmit = () =>{

    if (emailValidate())
    {
        emits('submit', email.value);
    }    
}

</script>
<style scoped lang="scss">
    .login-form
    {
        font-size: 1.2em;
        font-family: 'Segoe UI';
        .input-part
        {                                    
            margin: 2rem auto;        
        }
        .submit-login
        {
            max-width: 300px;
            display: block;
            margin: 0 auto;
            padding-left: 1.5em;
            padding-right: 1.5em;
        }
    }   
</style>