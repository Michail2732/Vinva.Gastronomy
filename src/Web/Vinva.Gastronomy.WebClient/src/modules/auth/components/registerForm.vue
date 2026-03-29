<template>
    <div class="register-form">
        <FloatLabel variant="over" class="input-part">
            <InputText id="email_lbl" @value-change="emailValidate" v-model="email" :invalid="!isEmailValid" fluid />
            <label for="email_lbl">Введите email</label>                                        
        </FloatLabel>
        <Message v-if="emailError" severity="error">{{ emailError }}</Message>
        <FloatLabel variant="over" class="input-part">
            <Password id="password_lbl" @value-change="passwordValidate" v-model="password" 
            :feedback="false" :invalid="!isPasswordValid" fluid />
            <label for="password_lbl">Придумайте пароль</label>            
        </FloatLabel>
        <Message v-if="passwordError" severity="error">{{ passwordError }}</Message>
        <FloatLabel variant="over" class="input-part">
            <Password id="confirm_pass_lbl" @value-change="passwordValidate" v-model="confirmPass" 
            :feedback="false" :invalid="!isConfirmPassValid" fluid  />
            <label for="confirm_pass_lbl">Повторите пароль</label>            
        </FloatLabel>
        <Message v-if="confirmPassError" severity="error">{{ confirmPassError }}</Message>
        <Button label="Зарегистрироваться" @click="handleSubmit" class="submit-register" />
    </div>
</template>
<script setup lang="ts">
import {ref} from 'vue'

const password = ref('');
const isPasswordValid = ref(true);
const passwordError = ref('');

const confirmPass = ref('');
const isConfirmPassValid = ref(true);
const confirmPassError = ref('');

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

const passwordValidate = () => 
{    
    isPasswordValid.value = !password.value || password.value == confirmPass.value;
    passwordError.value = isPasswordValid.value ? '' : (!password.value ? 'Пароль должен быть заполнен': 'Пароли должны совпадать');
    isConfirmPassValid.value = !confirmPass.value || password.value == confirmPass.value;
    confirmPassError.value = isConfirmPassValid.value ? '' : (!confirmPass.value ? 'Пароль должен быть заполнен': 'Пароли должны совпадать')
    return isPasswordValid.value && isConfirmPassValid.value;
}

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
    submit: [email: string, password: string]
}>();

const handleSubmit = () =>{

    if (emailValidate() && passwordValidate())
    {
        emits('submit', email.value, password.value);
    }    
}

</script>
<style scoped lang="scss">
    .register-form
    {
        text-align: center;        

        .progress-overlay
        {
            position: absolute;
            z-index: 10;            
            display: flex;
            left: 50%;
            top: 50%;   
            transform: translate(-50%, 0%);                 
        }       
        .input-part
        {                                    
            margin: 2rem auto 0 auto;        
        }                        
        .submit-register
        {
            max-width: 300px;
            display: block;
            margin: 2rem auto 0 auto;
            padding-left: 1.5em;
            padding-right: 1.5em;
        }        
        .msg-error-enter-active,
        .msg-error-leave-active 
        {
            transition: opacity 0.3s ease;
        }
        .msg-error-enter-from,
        .msg-error-leave-to 
        {
            opacity: 0;
        }        
    }   
</style>