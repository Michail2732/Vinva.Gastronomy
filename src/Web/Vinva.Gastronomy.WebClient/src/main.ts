import { createApp } from 'vue'
import { createPinia } from 'pinia'
import primeVuePlagin from './plugins/primeVuePlagin';

import App from './App.vue'
import router from './router'

const app = createApp(App)

app.use(createPinia())
app.use(router)
app.use(primeVuePlagin);

app.mount('#app')
