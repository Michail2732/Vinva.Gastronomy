<template>
  <header class="navbar">
    <div class="navbar-container">      
      <div class="navbar-left">        
        <Button 
          v-if="isMobile"
          icon="pi pi-bars"
          class="navbar-burger p-button-text"
          @click="$emit('toggle-mobile-menu')"/>        
        <!-- Логотип -->
        <router-link to="/" class="navbar-logo">
          <img src="/logo.svg" alt="Лого" height="40">
          <span class="navbar-logo-text hidden sm:inline">Кулинарный клуб</span>
        </router-link>
      </div>            
      
      <!-- Правая часть: меню и профиль -->
      <div class="navbar-right">
        <!-- Навигационные ссылки (десктоп) -->
        <div class="navbar-links hidden md:flex">
          <router-link to="/recipes" class="navbar-link" active-class="active">
            Рецепты
          </router-link>
          <router-link to="/ingredients" class="navbar-link" active-class="active">
            Ингредиенты
          </router-link>
          <router-link to="/about" class="navbar-link" active-class="active">
            О нас
          </router-link>
        </div>
        
        <!-- Меню пользователя -->
        <div v-if="user" class="navbar-user">
          <Button 
            icon="pi pi-heart"
            class="p-button-rounded p-button-text p-button-sm"
            @click="goToFavorites"
          />
          
          <Menu ref="menu" :v-model="userMenuItems" popup>
            <template #start>
              <div class="p-3 border-bottom">
                <div class="font-bold">{{ user.name }}</div>
                <div class="text-sm text-color-secondary">{{ user.email }}</div>
              </div>
            </template>
          </Menu>                    
        </div>
        
        <!-- Кнопки входа/регистрации -->
        <div v-else class="navbar-auth">
          <Button 
            label="Вход"
            class="p-button-text p-button-sm"
            @click="goToLogin"
          />
          <Button 
            label="Регистрация"
            class="p-button-sm"
            @click="goToRegister"
          />
        </div>
      </div>
    </div>
    
    <!-- Поиск на мобилках (под шапкой) -->
    <div v-if="isMobile" class="navbar-search-mobile">
      <span class="p-input-icon-left w-full">
        <i class="pi pi-search" />
        <InputText 
          v-model="searchQuery"
          placeholder="Поиск рецептов..."
          class="w-full"
          @keyup.enter="search"
        />
      </span>
    </div>
  </header>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/authStore'

const router = useRouter()
const authStore = useAuthStore()
const menu = ref()

// Состояние
const searchQuery = ref('')
const isMobile = ref(window.innerWidth < 768)

// Данные пользователя
const user = computed(() => authStore.user)

// Меню пользователя
const userMenuItems = ref([
  {
    label: 'Профиль',
    icon: 'pi pi-user',
    command: () => router.push('/profile')
  },
  {
    label: 'Мои рецепты',
    icon: 'pi pi-book',
    command: () => router.push('/my-recipes')
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

// Методы
const toggleMenu = (event: Event) => {
  menu.value.toggle(event)
}

const search = () => {
  if (searchQuery.value.trim()) {
    router.push({ 
      name: 'recipes', 
      query: { search: searchQuery.value }
    })
  }
}

const goToFavorites = () => router.push('/favorites')
const goToLogin = () => router.push('/login')
const goToRegister = () => router.push('/register')

defineEmits(['toggle-mobile-menu'])
</script>

<style scoped lang="scss">
.navbar {
  background: white;
  box-shadow: 0 2px 8px rgba(0,0,0,0.05);
  position: sticky;
  top: 0;
  z-index: 100;
}

.navbar-container {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 0.75rem 1.5rem;
  max-width: 1400px;
  margin: 0 auto;
}

.navbar-left {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.navbar-logo {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  text-decoration: none;
  color: #1e293b;
  font-weight: 600;
}

.navbar-search {
  flex: 1;
  max-width: 400px;
  margin: 0 2rem;
}

.navbar-right {
  display: flex;
  align-items: center;
  gap: 1.5rem;
}

.navbar-links {
  gap: 1.5rem;
}

.navbar-link {
  text-decoration: none;
  color: #64748b;
  font-weight: 500;
  transition: color 0.2s;
  
  &:hover,
  &.active {
    color: #f97316;
  }
}

.navbar-user {
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.navbar-avatar {
  cursor: pointer;
}

.navbar-auth {
  display: flex;
  gap: 0.5rem;
}

.navbar-search-mobile {
  padding: 0.5rem 1rem;
  border-top: 1px solid #e2e8f0;
}

// Адаптивность
@media (max-width: 768px) {
  .navbar-container {
    padding: 0.5rem 1rem;
  }
}
</style>