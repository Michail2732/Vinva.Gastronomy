import {defineStore} from 'pinia'
import { ref, computed } from 'vue'
import type { AuthOperationResult, User } from '@/modules/auth/types/authTypes'
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

  function getErrorMessage(err: any, defaultMessage: string = 'Ошибка аутентификации') : string 
  {
      return  err.response?.data?.message || err.Message || String(err) || defaultMessage;
  }

  // Вход
  async function loginUser(login: string, password: string): Promise<AuthOperationResult> {        
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
      var fetchRes = await fetchUser();
      if (!fetchRes.isSuccess)
        throw new AuthError(fetchRes.erros!);
      
      // Редирект на главную или на запрошенную страницу
      const redirect = router.currentRoute.value.query.redirect as string
      router.push(redirect || '/')
      
      return {isSuccess: true, erros: null}
    } catch (err: any) {      
      return {isSuccess: true, erros: getErrorMessage(err, 'Ошибка входа')}
    } 
  }

  // Регистрация
  async function registerUser(login: string, email: string, password: string ): Promise<AuthOperationResult>
  {
    try
    {      
      const responce = await registrationRegister(
        {
          body:
          {
            email: email,
            login: login,
            password: password
          }
        }
      );
      return {isSuccess: true, erros: null}
    } catch (err) {
      return {isSuccess: false, erros: getErrorMessage(err, 'Ошибка при регистрации')}
    }    
  }

  // Загрузка пользователя
  async function fetchUser(): Promise<AuthOperationResult> {
    if (!token.value) 
      return {isSuccess: false, erros: 'Пользователь не аутентифицирован'}
    
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
      return {isSuccess: true, erros: null}
    } 
    catch (err: any) {      
        await logoutUser()              
        return {isSuccess: false, erros: getErrorMessage(err, 'Ошибка получения пользователя')}
    }
  }

  // Выход
  async function logoutUser(): Promise<AuthOperationResult>  {
    if (!token.value) 
      return {isSuccess: true, erros: null};
    try
    {
      user.value = null
      token.value = null
      await authenticationLogout();
      router.push('/login');
      return {isSuccess: true, erros: null};
    } catch (err) {
      return {isSuccess: false, erros: getErrorMessage(err, 'Ошибка получения пользователя')}
    }    
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