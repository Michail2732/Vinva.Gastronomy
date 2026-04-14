import { createApp } from 'vue'
import { createPinia } from 'pinia'
import primeVuePlagin from './plugins/primeVuePlagin';
import App from './App.vue'
import router from './router'
import '@/assets/main.scss'
import '@/plugins/fontsSettings.ts'

const app = createApp(App)

app.use(createPinia())
app.use(router)
app.use(primeVuePlagin);
import '@/api/index'
app.mount('#app')
