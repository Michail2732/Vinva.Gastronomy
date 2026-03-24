<template >
    <div class="login-form">        
        <FloatLabel variant="over" class="input-part">            
            <InputText id="login_lbl"
                       v-model="login"
                       fluid />        
            <label class="label-head"
                   for="login_lbl">Введите логин</label>
        </FloatLabel>            
        <FloatLabel variant="over" class="input-part">            
            <InputText id="pass_lbl"
                       v-model="password"
                       type="password" fluid />
            <label class="label-head"
                   for="pass_lbl">Введите пароль</label>
        </FloatLabel>
        <Button label="Войти"
                @click="submitLogin"
                class="submit-login"/>
    </div>
</template>
<script setup lang="ts">
import {ref} from 'vue';
import {useAuthStore} from '@/modules/auth/stores/authStore'

const login = ref('')
const password= ref('')
const errorMessage = ref('');
const authStore = useAuthStore()
async function submitLogin()
{    
    const isSuccess = await authStore.loginUser(login.value, password.value);
    if (!isSuccess)
        errorMessage.value = 'Неверные логин или пароль'
}

</script>
<style lang="scss">    
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