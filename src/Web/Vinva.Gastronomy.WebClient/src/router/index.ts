import AuthLayout from '@/components/layouts/authLayout.vue'
import AuthPage from '@/modules/auth/pages/loginPage.vue'
import { createRouter, createWebHistory } from 'vue-router'

const routes = [
  {
    path: '/',    
    component: AuthLayout,
    children: [
      {
        path: '',
        component: AuthPage
      }
    ]
  }
];

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: routes,
})

export default router
