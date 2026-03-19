import {defineStore} from 'pinia'
import { ref, computed } from 'vue'
import type { User } from './types'
import {AuthenticationService} from '@/api/services/AuthenticationService'
import type { LoginRequest } from '@/api/models/LoginRequest';
import type { LoginResponceDto } from '@/api/models/LoginResponceDto';
import type { LogoutRequest } from '@/api/models/LogoutRequest';

import router from '@/router'

export const useAuthStore = defineStore('auth', () => {
  // ===== STATE =====
  const user = ref<User | null>(null)
  const token = ref<string | null>(localStorage.getItem('token'))
  const isLoading = ref(false)
  const error = ref<string | null>(null)

  // ===== GETTERS =====
  const isAuthenticated = computed(() => !!token.value && !!user.value)  
  const userName = computed(() => user.value?.name || '')

  // ===== ACTIONS =====
  
  // Вход
  async function login(login: string, password: string) {
    isLoading.value = true
    error.value = null
    
    try {
      const response = await AuthenticationService.postLogin(
        {
          login: login,
          password: password
        }
      )
      
      // Сохраняем токен
      token.value = response.accessToken
      localStorage.setItem('token', response.)
      
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
  async function register(userData: { name: string; email: string; password: string }) {
    isLoading.value = true
    error.value = null
    
    try {
      const response = await AuthenticationService.register(userData)
      
      token.value = response.token
      localStorage.setItem('token', response.token)
      
      await fetchUser()
      router.push('/')
      
      return true
    } catch (err: any) {
      error.value = err.response?.data?.message || 'Ошибка регистрации'
      return false
    } finally {
      isLoading.value = false
    }
  }

  // Загрузка пользователя
  async function fetchUser() {
    if (!token.value) return
    
    try {
      const userData = await AuthenticationService.getCurrentUser()
      user.value = userData
    } catch (err) {
      logout()
    }
  }

  // Выход
  function logout() {
    user.value = null
    token.value = null
    localStorage.removeItem('token')
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
    login,
    register,
    logout,
    fetchUser    
  }
})