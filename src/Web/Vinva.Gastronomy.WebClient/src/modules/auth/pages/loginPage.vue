<template >
    <div class="auth-layout">        
        <div class="auth-container">
            <!-- Логотип -->
            <div class="auth-logo">
                <Image src="/logo.svg" alt="logo" class="logo-img" />
            </div>

            <!-- Карточка с формой -->
            <Card class="auth-card">
                <template #content>                    
                    <LoginForm @submit="submitLogin"
                               :isLoginProgress="isloginInProgress"
                               :authError="authErrors"/>
                </template>                
            </Card>

            <!-- Ссылка на главную -->
            <div class="auth-footer">
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
import { useRouter, useRoute } from 'vue-router';
import LoginForm from '../components/loginForm.vue'
import {useAuthStore} from '@/modules/auth/stores/authStore'

const router = useRouter();
const route = useRoute();
const authStore = useAuthStore()
const authErrors = ref('');
const isloginInProgress = ref(false);

async function submitLogin(login: string, password: string)
{    
    try 
    {
        isloginInProgress.value = true;
        authErrors.value = '';
        const loginResult = await authStore.loginUser(login, password);
        if (!loginResult.isSuccess)
        {            
            authErrors.value = loginResult.error ?? 'Неверные логин или пароль';
        }    
        else
        {            
            const redirectPath = route.query.redirect as string || '/Home';
            router.push(redirectPath);
        }        
    } 
    finally
    {
        isloginInProgress.value = false;
    }    
}

</script>
<style scoped lang="scss">       
.auth-layout {
    min-height: 100vh;
    display: flex;
    align-items: center;
    justify-content: center;
    position: relative;    
}

.auth-container {
    width: 100%;
    max-width: 450px;
    padding: 1.5rem;
    position: relative;
    z-index: 1;

    .auth-logo {        
        margin-bottom: 2rem;
        .logo-img :deep(img)
        {
            max-width: 400px;
            margin: 0 auto;
            color: white;
        }
    }

    .auth-card 
    {        
        min-height: 250px;
        border-radius: 1rem;
        padding: 1rem;
        box-shadow: 0px 3px 24px 3px rgba(34, 60, 80, 0.2);  
    }

    .auth-footer {
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
    .auth-container {
        padding: 1rem;
    }

    .auth-card {
        padding: 1.5rem;
    }

    .auth-logo .logo-link {
        font-size: 1.25rem;

        .logo-icon {
            font-size: 1.5rem;
        }
    }
}
</style>