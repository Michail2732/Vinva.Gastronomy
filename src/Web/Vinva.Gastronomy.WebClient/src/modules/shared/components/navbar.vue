<template>
  <header class="navbar">
      <Menubar :model="userMenuItems"/>
  </header>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/modules/auth/stores/authStore'

const router = useRouter()
const authStore = useAuthStore()


// Данные пользователя
const user = computed(() => authStore.user)

// Меню пользователя
const userMenuItems = ref([
  {
    label: 'Profile',
    icon: 'pi pi-user',
    command: () => router.push('/profile')
  },
  {
    label: 'Рецепты',
    icon: 'pi pi-book',
    command: () => router.push('/recipes')
  },
  {
    label: 'Избранное',
    icon: 'pi pi-heart',
    command: () => router.push('/favorites')
  },
  {
    separator: true
  },
  {
    label: 'Настройки',
    icon: 'pi pi-cog',
    command: () => router.push('/settings')
  },
  {
    label: 'Выйти',
    icon: 'pi pi-sign-out',
    command: () => authStore.logoutUser()
  }
])

const goToRecipes = () => router.push('/recipes')
const goToLogin = () => router.push('/login')
const goToRegister = () => router.push('/register')

</script>

<style scoped lang="scss">

</style>