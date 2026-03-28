import AppLayout from '@/AppLayout.vue'
import LoginPage from '@/modules/auth/pages/loginPage.vue'
import RegisterPage from '@/modules/auth/pages/registerPage.vue'
import RecipesPage from '@/modules/recipes/pages/recipesPage.vue'
import { createRouter, createWebHistory } from 'vue-router'

const routes = [
  {
    path: '/',    
    redirect: '/home',    
  },
  {
    path: '/home',    
    component: AppLayout,    
  },
  {
    path: '/login',    
    component: LoginPage,
  },
  {
    path: '/register',    
    component: RegisterPage,
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
