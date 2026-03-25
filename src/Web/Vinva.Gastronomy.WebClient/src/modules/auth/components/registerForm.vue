<template >
    <div class="login-form">
        <FloatLabel variant="over" class="input-part">
            <InputText id="email_lbl" @input="handleInputEmail" v-model="email" 
             :invalid="!!emailError" fluid />
            <label class="label-head" for="email_lbl">Введите email</label>
            <Message v-if="!emailError" severity="error">{{ emailError }}</Message>
        </FloatLabel>        
        <Button label="Войти" @click="handleSubmit" class="submit-login" />
    </div>
</template>
<script setup lang="ts">
import {ref, computed} from 'vue'


const email = ref('');
const emailError = computed(() => {
    const emailRegex =/^[^\s@]+@([^\s@]+\.)+[^\s@]+$/
    if (!email.value || !emailRegex.test(email.value))
    {
        return 'Некорректный email';
    }    
    return null;        
});

const emits = defineEmits<{
    submit: [email: string]
}>();


const handleInputEmail = () => emailError.value;

const handleSubmit = () =>{

    if (!emailError)
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