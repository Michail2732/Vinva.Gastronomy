<template>
    <div class="login-form">
        <FloatLabel variant="over" class="input-part">
            <InputText id="login_lbl" v-model="login" fluid />
            <label class="label-head" for="login_lbl">Введите логин</label>
        </FloatLabel>
        <FloatLabel variant="over" class="input-part">
            <InputText id="pass_lbl" v-model="pass" type="password" fluid />
            <label class="label-head" for="pass_lbl">Введите пароль</label>
        </FloatLabel>
        <Button label="Войти" @click="handleSubmit" class="submit-login" />
    </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'

const login = ref('');
const pass = ref('');

const emits = defineEmits<{
    submit: [login: string, pass: string];
    cancel: [];
}>();

const handleSubmit = () => {
    emits('submit', login.value, pass.value);
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