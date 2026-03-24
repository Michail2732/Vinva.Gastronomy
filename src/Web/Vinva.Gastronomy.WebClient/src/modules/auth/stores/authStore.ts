import {defineStore} from 'pinia'
import { ref, computed } from 'vue'
import type { User } from '../../../stores/types'
import {authenticationLogin, authenticationLogout, authenticationMe, 
  authenticationRefreshToken, authenticationValidateToken, registrationRegister} from '@/api/sdk.gen'
import router from '@/router'
import { AuthError } from '@/types'

export const useAuthStore = defineStore('auth', () => {
  // ===== STATE =====
  const user = ref<User | null>(null)
  const token = ref<string | null>(null)
  const isLoading = ref(false)
  const error = ref<string | null>(null)

  // ===== GETTERS =====
  const isAuthenticated = computed(() => !!token.value && !!user.value)  
  const userName = computed(() => user.value?.name || '')

  // ===== ACTIONS =====
  
  // Вход
  async function loginUser(login: string, password: string) {
    isLoading.value = true
    error.value = null
    
    try {
      const response = await authenticationLogin(
        {
          body: {
            login: login,
            password: password
          }
        }
      )
      if (!response.data?.accessToken)
        throw new AuthError("Проблемы c сервером");
      // Сохраняем токен
      token.value = response.data.accessToken;
      
      // Загружаем пользователя
      await fetchUser()
      
      // Редирект на главную или на запрошенную страницу
      const redirect = router.currentRoute.value.query.redirect as string
      router.push(redirect || '/')
      
      return true
    } catch (err: any) {
      error.value = err.response?.data?.message || 'Ошибка входа'
      return false
    } finally {
      isLoading.value = false
    }
  }

  // Регистрация
  async function registerUser(login: string, email: string, password: string ) 
  {

    
  }

  // Загрузка пользователя
  async function fetchUser() {
    if (!token.value) return
    
    try {
      const userData = await authenticationMe()
      if (!userData.data)
        throw new AuthError("Проблемы c сервером");
      user.value = 
      {
        id: userData.data.id,
        name: userData.data.login,
        email: userData.data.email,
        roles: userData.data.roles
      }
    } catch (err) {
      await logoutUser()
    }
  }

  // Выход
  async function logoutUser() {
    user.value = null
    token.value = null
    await authenticationLogout();    
    router.push('/login')
  }
  

  return {
    // State
    user,
    token,
    isLoading,
    error,
    
    // Getters
    isAuthenticated,    
    userName,    
    
    // Actions
    loginUser,
    registerUser,
    logoutUser,
    getUser: fetchUser    
  }
})