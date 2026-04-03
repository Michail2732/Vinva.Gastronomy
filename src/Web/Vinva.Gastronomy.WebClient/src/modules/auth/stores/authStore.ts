import {defineStore} from 'pinia'
import { ref, computed } from 'vue'
import type { AuthOperationResult, User } from '@/modules/auth/types/authTypes'
import {authenticationLogin, authenticationLogout, 
  authenticationMe, registrationRegister} from '@/api/gastronomy_generated/sdk.gen'
import router from '@/router'
import type { UserInfoDto } from '@/api/gastronomy_generated/types.gen'
import type { ApiResult } from '@/api/types'
import { safeApiCall } from '@/api/utils'

export const useAuthStore = defineStore('auth', () => {
  // ===== STATE =====
  const user = ref<User | null>(null)    
  // ===== GETTERS =====
  const isAuthenticated = computed(() => user.value != null)    

  // ===== ACTIONS =====

  function getErrorMessage(err: any, defaultMessage: string = 'Ошибка аутентификации') : string 
  {
      return  err.response?.data?.message || err.Message || String(err) || defaultMessage;
  }

  function setUser(userInfo: UserInfoDto )
  {
    user.value =
    {
      id: userInfo.id,
      email: userInfo.email,
      login: userInfo.login,
      roles: userInfo.roles,
      state: userInfo.state
    };
  }
  // Вход
  async function loginUser(login: string, password: string): Promise<ApiResult> {        
    try {      
      const response = await authenticationLogin(
        {
          body: {
            login: login,
            password: password
          }
        }
      )            
      setUser(response.data!);
      
      // Редирект на главную или на запрошенную страницу
      const redirect = router.currentRoute.value.query.redirect as string
      router.push(redirect || '/')
      
      return {isSuccess: true}
    } catch (err: any) {
      return {isSuccess: true, error: getErrorMessage(err, 'Ошибка входа')}
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
      return {isSuccess: true, error: null, details: responce.data?.details}
    } catch (err) {
      return {isSuccess: false, error: getErrorMessage(err, 'Ошибка при регистрации')}
    }    
  }

  // Загрузка пользователя
  async function fetchUser(): Promise<AuthOperationResult> {        
    try {
      const responce = await authenticationMe()
      setUser(responce.data!)      
      return {isSuccess: true, error: null}
    } 
    catch (err: any) {      
        await logoutUser()              
        return {isSuccess: false, error: getErrorMessage(err, 'Ошибка получения пользователя')}
    }
  }

  // Выход
  async function logoutUser(): Promise<AuthOperationResult>  {    
    try
    {            
      await authenticationLogout();
      user.value = null      
      router.push('/login');
      return {isSuccess: true, error: null};
    } catch (err) {
      return {isSuccess: false, error: getErrorMessage(err, 'Ошибка получения пользователя')}
    }    
  }
  

  return {
    // State
    user,    
    
    // Getters
    isAuthenticated,    
    
    // Actions
    loginUser,
    registerUser,
    logoutUser,
    getUser: fetchUser    
  }
})