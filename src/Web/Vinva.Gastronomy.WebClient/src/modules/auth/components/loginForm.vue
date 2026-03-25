<template >
    <div class="login-form">
        <Message v-if="authError" severity="error">{{ authError }}</Message>
        <FloatLabel variant="over" class="input-part">            
            <InputText id="login_lbl"
                       v-model="login"                                         
                       :invalid="loginIsValid"
                       fluid />        
            <label class="label-head"
                   for="login_lbl">Введите логин</label>                        
        </FloatLabel>            
        <Message v-if="loginIsValid" severity="error">{{ loginError }}</Message>
        <FloatLabel variant="over" class="input-part">            
            <Password id="pass_lbl"
                       v-model="pass"                                                                   
                       :invalid="passIsValid" 
                       fluid/>
            <label class="label-head"
                   for="pass_lbl">Введите пароль</label>                        
        </FloatLabel>
        <Message v-if="passIsValid" severity="error">{{ passError }}</Message>
        <Button label="Войти"
                :disabled="canSubmit"
                @click="handleSubmit"
                class="submit-login"/>
    </div>
</template>
<script setup lang="ts">
    import type { InputText, Password } from 'primevue';
    import { ref, computed  } from 'vue';    

    const authError = ref('');

    const login = ref('');
    const loginIsValid = ref(true);
    const loginError = computed(() => {
        if (!login.value)
        {
            loginIsValid.value = false;
            return 'Необходимо ввести логин';   
        }        
        loginIsValid.value = true;
        return null;
    });    

    const pass = ref('');
    const passIsValid = ref(true);
    const passError = computed(() => {
        if (!pass.value)
        {
            passIsValid.value = false;
            return 'Необходимо ввести пароль';   
        }         
        passIsValid.value = true;
        return null;
    });    

    const defProps = defineProps<{authError: string}>();

    const emits = defineEmits<{
        submit: [login: string, pass: string]        
    }>();    

    const canSubmit = computed(() => !loginError && !passError) 
    const handleSubmit = () => {
        if (!canSubmit)            
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