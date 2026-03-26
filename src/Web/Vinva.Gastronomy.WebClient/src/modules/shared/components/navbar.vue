<template>
  <header class="navbar">      
      <Menubar :model="userMenuItems">
        <template #start>
          <Image src="/logo-short.svg" alt="logo" class="logo-small-img" />
        </template>
        <template #end>
          <Avatar/>
        </template>
      </Menubar>
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
    label: 'Рецепты',
    icon: 'pi pi-book',
    command: () => router.push('/recipes')
  },
  {
    label: 'Ингредиенты',
    icon: 'pi pi-heart',
    command: () => router.push('/favorites')
  },    
  {
    label: 'Справка',
    icon: 'pi pi-sign-out',
    command: () => authStore.logoutUser()
  }
])

const goToRecipes = () => router.push('/recipes')
const goToLogin = () => router.push('/login')
const goToRegister = () => router.push('/register')

</script>

<style scoped lang="scss">
  .logo-small-img
  {
      display: block;
      
  }

  .logo-small-img :deep(img)
  {
      display: block;
      height: 2rem;
      
  }

</style>