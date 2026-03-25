import AppLayout from '@/AppLayout.vue'
import LoginPage from '@/modules/auth/pages/loginPage.vue'
import RecipesPage from '@/modules/recipes/pages/recipesPage.vue'
import { createRouter, createWebHistory } from 'vue-router'

const routes = [
  {
    path: '/',    
    component: AppLayout,    
  },
  {
    path: '/login',    
    component: LoginPage,
  },
  {
    path: '/register',    
    component: LoginPage,
  },
  {
    path: '/recipes',    
    component: RecipesPage,
  },
];

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: routes,
})

export default router
