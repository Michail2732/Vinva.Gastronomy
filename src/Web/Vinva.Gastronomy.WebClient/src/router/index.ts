import AppLayout from '@/AppLayout.vue'
import LoginPage from '@/modules/auth/pages/loginPage.vue'
import RegisterPage from '@/modules/auth/pages/registerPage.vue'
import RecipesPage from '@/modules/recipes/pages/recipesPage.vue'
import { createRouter, createWebHistory } from 'vue-router'

const routes = [
  {
    path: '/',        
    component: AppLayout,
    children:
    [
      {
        path: '',        
        redirect: '/home',        
      },
      {
        path: '/home',
        name: 'home',        
        component: () => import('@/modules/shared/components/appHome.vue')
      },
      {
        path: '/recipes',
        name: 'recipes',
        component: RecipesPage,
      }
    ]
  },  
  {
    path: '/login',    
    component: LoginPage,
  },
  {
    path: '/register',    
    component: RegisterPage,
  },  
];

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: routes,
})

export default router
