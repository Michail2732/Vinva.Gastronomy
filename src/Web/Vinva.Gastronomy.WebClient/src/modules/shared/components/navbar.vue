<template>
  <header class="navbar">
    <div class="menubar-custom" :class="{ 'mobile-open': isMenuOpen }">
      <!-- Десктопная версия -->
      <div class="desktop-menu">
        <div class="menubar-left">
          <Image src="/logo.svg" alt="logo" class="logo-img" />
        </div>
        
        <div class="menubar-center-wrapper">
          <div class="menubar-center">
            <Button 
              v-for="item in userMenuItems" 
              :key="item.label"
              class="menubar-item"
              :label="item.label"
              :icon="item.icon"
              link
              @click="handleItemClick(item)"
            />
          </div>
        </div>
        
        <div class="menubar-right">
          <!-- Аутентифицирован -->
          <div v-if="authStore.isAuthenticated" class="user-info">
            <Avatar               
              class="user-avatar"
              shape="circle"
              v-tooltip.top="userName"
            />
            <Chip :label="userName" class="user-name-chip" v-tooltip.top="userName" />
            <Button 
              icon="pi pi-sign-out" 
              label="Выйти"
              severity="secondary"
              text
              @click="handleLogout"
              class="logout-btn"
            />
          </div>
          
          <!-- Не аутентифицирован -->
          <Button 
            v-else
            label="Войти"
            icon="pi pi-sign-in"
            severity="primary"
            @click="goToLogin"
          />
        </div>
      </div>

      <!-- Мобильная версия -->
      <div class="mobile-menu">
        <div class="mobile-header">
          <Image src="/logo.svg" alt="logo" class="logo-img-mobile" />
          <div class="mobile-controls">
            <Avatar 
              v-if="authStore.isAuthenticated"              
              class="user-avatar-mobile"
              shape="circle"
              v-tooltip.top="userName"
            />
            <Button 
              v-else
              label="Войти"
              icon="pi pi-sign-in"
              size="small"
              text
              @click="goToLogin"
            />
            <Button 
              :icon="isMenuOpen ? 'pi pi-times' : 'pi pi-bars'"
              class="burger-btn"
              text
              @click="toggleMenu"
            />
          </div>
        </div>
        
        <div class="mobile-nav" v-if="isMenuOpen">
          <Button 
            v-for="item in userMenuItems" 
            :key="item.label"
            class="mobile-nav-item"
            :label="item.label"
            :icon="item.icon"
            text
            @click="handleItemClick(item)"
          />
          
          <Button 
            v-if="authStore.isAuthenticated"
            label="Выйти"
            icon="pi pi-sign-out"
            class="mobile-logout-btn"
            text
            severity="danger"
            @click="handleLogout"
          />
        </div>
      </div>
    </div>
  </header>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/modules/auth/stores/authStore'
import Button from 'primevue/button'
import Avatar from 'primevue/avatar'
import Image from 'primevue/image'
import Chip from 'primevue/chip'

const router = useRouter()
const authStore = useAuthStore()
const isMenuOpen = ref(false)

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
    icon: 'pi pi-question-circle',
    command: () => router.push('/help')
  }
])

const userName = computed(() => authStore.user?.name || authStore.user?.email || 'Пользователь')


const handleItemClick = (item: any) => {
  item.command()
  isMenuOpen.value = false
}

const toggleMenu = () => {
  isMenuOpen.value = !isMenuOpen.value
}

const handleLogout = async () => 
{
  await authStore.logoutUser();
  isMenuOpen.value = false;
  router.push('/');
}

const goToLogin = () => {
  router.push('/login');
}
</script>

<style scoped lang="scss">
.menubar-custom {
  background: var(--surface-card);
  border-radius: 6px;
  width: 100%;
}

// Десктопная версия
.desktop-menu {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 0.75rem 1rem;
  position: relative;
  
  .menubar-left {
    flex-shrink: 0;
  }
  
  .menubar-center-wrapper {
    position: absolute;
    left: 0;
    right: 0;
    display: flex;
    justify-content: center;
    pointer-events: none;
  }
  
  .menubar-center {
    display: flex;
    gap: 1.5rem;
    pointer-events: auto;
  }
  
  .menubar-right {
    flex-shrink: 0;
    margin-left: auto;
  }
}

.menubar-item {
  color: var(--text-color);
  &:deep(.p-button-label) {
    font-weight: 500;    
  }
}

.user-info {
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.user-avatar {
  cursor: pointer;
  transition: transform 0.2s;
  background: var(--primary-color);
  
  &:hover {
    transform: scale(1.05);
  }
}

.user-name-chip {
  &:deep(.p-chip) {        
    padding: 0.5rem 0.75rem;
    max-width: 150px;
    
    .p-chip-label {
      overflow: hidden;
      text-overflow: ellipsis;
      white-space: nowrap;
    }
  }
}

.logo-img {
  :deep(img) {
    height: 2.4rem;
    display: block;
  }
}

// Мобильная версия
.mobile-menu {
  display: none;
}

.mobile-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 0.75rem 1rem;
}

.mobile-controls {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.user-avatar-mobile {
  cursor: pointer;
  background: var(--primary-color);
}

.burger-btn {
  &:deep(.p-button) {
    color: var(--text-color-secondary);
    
    &:hover {
      color: var(--primary-color);
    }
  }
}

.mobile-nav {
  display: flex;
  flex-direction: column;
  padding: 1rem;
  gap: 0.5rem;
  border-top: 1px solid var(--surface-border);
}

.mobile-nav-item {
  justify-content: flex-start !important;
  width: 100%;
  
  &:deep(.p-button-label) {
    flex: 1;
    text-align: left;
  }
}

.mobile-logout-btn {
  margin-top: 0.5rem;
  justify-content: flex-start !important;
  width: 100%;
  
  &:deep(.p-button-label) {
    flex: 1;
    text-align: left;
  }
}

.logo-img-mobile {
  :deep(img) {
    height: 2rem;
    display: block;
  }
}

// Адаптив
@media (min-width: 769px) {
  .mobile-menu {
    display: none !important;
  }
  
  .desktop-menu {
    display: flex !important;
  }
}

@media (max-width: 768px) {
  .desktop-menu {
    display: none !important;
  }
  
  .mobile-menu {
    display: block !important;
  }
}
</style>