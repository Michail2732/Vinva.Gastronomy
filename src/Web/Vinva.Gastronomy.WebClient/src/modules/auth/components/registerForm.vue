<template >
    <div class="login-form">
        <FloatLabel variant="over" class="input-part">
            <InputText id="email_lbl" v-model="email" fluid />
            <label class="label-head" for="email_lbl">Введите логин</label>
        </FloatLabel>        
        <Button label="Войти" @click="handleSubmit" class="submit-login" />
    </div>
</template>
<script setup lang="ts">
import {ref, computed} from 'vue'


const email = ref('');
function validateEmail() : boolean
{
    const emailRegex =/^[^\s@]+@([^\s@]+\.)+[^\s@]+$/
    if (!email.value || !emailRegex.test(email.value))
    {
        return false;
    }    
    return true;
}

const emailError = computed(() => {
    if (!validateEmail())
        return 'Некорректный email';
    return null;
});

const emits = defineEmits<{
    submit: [email: string]
}>();


const handleInputEmail = () => validateEmail();

const handleSubmit = () =>{

    if (validateEmail())
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