<template>
     <div class="register-layout">        
        <div class="register-container">
            <!-- Логотип -->
            <div class="register-logo">
                <Image src="/logo.svg" alt="logo" class="logo-img" />
            </div>

            <!-- Карточка с формой -->
            <Card class="register-card">
                <template #content>                    
                    <RegisterForm @submit="submitRegister"
                                  :isRegisterInProgress="isRegisterInProgress"
                                  :registerErrors="registerErrors"/>
                </template>                
            </Card>

            <!-- Ссылка на главную -->
            <div class="register-footer">
                <router-link to="/" class="back-link">
                    <i class="pi pi-home"></i>
                    Вернуться на главную
                </router-link>
            </div>
        </div>
    </div>    
</template>
<script setup lang="ts">
import {ref} from 'vue';
import { useRouter } from 'vue-router';
import {useAuthStore} from '@/modules/auth/stores/authStore'
import RegisterForm from '@/modules/auth/components/registerForm.vue'


const router = useRouter();
const authStore = useAuthStore()
const registerErrors = ref('');
const isRegisterInProgress = ref(false);

async function submitRegister(email: string, password: string)
{
    try 
    {
        isRegisterInProgress.value = true;
        registerErrors.value = '';
        const registerResult = await authStore.registerUser(email, email, password);
        if (!registerResult.isSuccess)
        {            
            registerErrors.value = registerResult.error ?? 'Неверные логин или пароль';
        }    
        else
        {            
            router.push('/Home');
        }        
    } 
    finally
    {
        isRegisterInProgress.value = false;
    }    
}
</script>
<style scoped lang="scss">
    .register-layout {
    min-height: 100vh;
    display: flex;
    align-items: center;
    justify-content: center;
    position: relative;    
}

.register-container {
    width: 100%;
    max-width: 450px;
    padding: 1.5rem;
    position: relative;
    z-index: 1;

    .register-logo {        
        margin-bottom: 2rem;
        .logo-img :deep(img)
        {
            max-width: 400px;
            margin: 0 auto;
            color: white;
        }
    }

    .register-card 
    {        
        min-height: 250px;
        border-radius: 1rem;
        padding: 1rem;
        box-shadow: 0px 3px 24px 3px rgba(34, 60, 80, 0.2);  
    }

    .register-footer {
        text-align: center;
        margin-top: 1.5rem;

        .back-link {
            display: inline-flex;
            align-items: center;
            gap: 0.5rem;
            color: var(--text-color);
            text-decoration: none;
            font-size: 0.875rem;
            transition: opacity 0.2s;

            &:hover {
                opacity: 0.8;
            }

            i {
                font-size: 0.875rem;
            }
        }
    }
}


@media (max-width: 640px) {
    .register-container {
        padding: 1rem;
    }

    .register-card {
        padding: 1.5rem;
    }

    .register-logo .logo-link {
        font-size: 1.25rem;

        .logo-icon {
            font-size: 1.5rem;
        }
    }
}
</style>