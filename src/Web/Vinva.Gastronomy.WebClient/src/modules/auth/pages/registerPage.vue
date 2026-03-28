<template>
     <div class="auth-layout">        
        <div class="auth-container">
            <!-- Логотип -->
            <div class="auth-logo">
                <Image src="/logo.svg" alt="logo" class="logo-img" />
            </div>

            <!-- Карточка с формой -->
            <Card class="auth-card">
                <template #content>                    
                    <RegisterForm @submit="submitRegister"
                                  :isRegisterInProgress="isRegisterInProgress"
                                  :registerErrors="registerErrors"/>
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
import { useRouter } from 'vue-router';
import {useAuthStore} from '@/modules/auth/stores/authStore'
import RegisterForm from '@/modules/auth/components/registerForm.vue'


const router = useRouter();
const authStore = useAuthStore()
const registerErrors = ref('');
const isRegisterInProgress = ref(false);

async function submitRegister()
{

}
</script>
<style scoped lang="scss">
    
</style>