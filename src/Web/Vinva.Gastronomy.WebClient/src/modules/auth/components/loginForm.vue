<template >
    <div class="login-form">
        <div class="progress-overlay">
            <ProgressSpinner v-if="isLoginProgress" />
        </div>        
        <Message v-show="authError" severity="error">{{ authError }}</Message>
        <FloatLabel variant="over" class="input-part">            
            <InputText id="login_lbl"
                       v-model="login"
                       :disabled="isLoginProgress"
                       autofocus 
                       @value-change="loginValidate"                                         
                       :invalid="!loginIsValid"
                       fluid />        
            <label class="label-head"
                   for="login_lbl">Введите логин</label>                        
        </FloatLabel> 
        <Transition class="msg-error-transition" name="msg-error">
            <Message class="msg-error"
                     v-if="!loginIsValid" 
                     severity="error">{{ loginError }}</Message>
        </Transition>                   
        <FloatLabel variant="over" class="input-part input-part-pass">            
            <Password id="pass_lbl"
                      :disabled="isLoginProgress"
                      @value-change="passValidate"
                      v-model="pass"                                                                   
                      :invalid="!passIsValid" 
                      fluid/>
            <label class="label-head"
                   for="pass_lbl">Введите пароль</label>                        
        </FloatLabel>
        <Transition name="msg-error" class="msg-error-transition">
            <Message v-if="!passIsValid" 
                     severity="error">{{ passError }}</Message>
        </Transition>        
        <Button label="Войти"     
                :disabled="!loginIsValid || !passIsValid || isLoginProgress"
                @click="handleSubmit"
                class="submit-login"/>
    </div>
</template>
<script setup lang="ts">
    import type { InputText, Password } from 'primevue';
    import { ref } from 'vue';        

    const login = ref('');
    const loginIsValid = ref(true);
    const loginError = ref('');
    const loginValidate = () => 
    {
        const isLoginEmpty = !login.value;
        loginIsValid.value = !isLoginEmpty;
        loginError.value = isLoginEmpty ? 'Необходимо ввести логин' : '';
        return !isLoginEmpty;                    
    };    

    const pass = ref('');
    const passIsValid = ref(true);
    const passError = ref('');
    const passValidate = () => 
    {
        const isPassEmpty = !pass.value;
        passIsValid.value = !isPassEmpty;
        passError.value = isPassEmpty ? 'Необходимо ввести пароль' : '';
        return !isPassEmpty;           
    };    

    const props = defineProps({
        authError: 
        {
            type: String,
            required: false,
            default: ''
        },
        isLoginProgress:
        {
            type: Boolean,
            required: false,
            default: false
        }
    });

    const emits = defineEmits<{
        submit: [login: string, pass: string]        
    }>();    
    
    const handleSubmit = () => {
        if (passValidate() && loginValidate())            
            emits('submit', login.value, pass.value);
    }            
</script>
<style scoped lang="scss">
    .login-form
    {
        min-height: 200px;
        font-size: 1.2em;
        font-family: 'Segoe UI'; 
        .progress-overlay
        {
            position: absolute;
            z-index: 10;            
            display: flex;
            left: 50%;
            top: 50%;   
            transform: translate(-50%, 50%);                 
        }       
        .input-part
        {                                    
            margin: 2rem auto 0 auto;        
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
        .submit-login
        {
            max-width: 300px;
            display: block;
            margin: 2rem auto 0 auto;
            padding-left: 1.5em;
            padding-right: 1.5em;
        }
    }    
</style>