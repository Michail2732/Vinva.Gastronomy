import { type App } from 'vue'
import PrimeVue from 'primevue/config'
import ToastService from 'primevue/toastservice'
import ConfirmationService from 'primevue/confirmationservice'
import Aura from '@primeuix/themes/aura'
import Button from 'primevue/button'
import InputText from 'primevue/inputtext'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import Card from 'primevue/card'
import Dialog from 'primevue/dialog'
import Toast from 'primevue/toast'
import ConfirmDialog from 'primevue/confirmdialog'
import MultiSelect from 'primevue/multiselect'
import FloatLabel from 'primevue/floatlabel'
import Rating from 'primevue/rating'
import Tag from 'primevue/tag'
import ProgressSpinner from 'primevue/progressspinner'
import Image from 'primevue/image'
import Avatar from 'primevue/avatar'
import Badge from 'primevue/badge'
import Menubar from 'primevue/menubar'
import Paginator from 'primevue/paginator'
import Message from 'primevue/message'
import Dock from 'primevue/dock'
import Password from 'primevue/password'
import 'primeicons/primeicons.css'

export default {
  install(app: App) {
    app.use(PrimeVue, 
      { 
        ripple: true,
        unstyled: false,
        theme: {
          preset: Aura,
          options: 
          {
            prefix: 'p',
            darkModeSelector: 'system',
            cssLayer: false,
            variables: 
            {            
              fontFamily: '"Inter", system-ui, -apple-system, "Segoe UI", Roboto, "Helvetica Neue", sans-serif',
              fontSize: '1rem',
              fontWeight: '500',              
              menu: {
                fontFamily: '"Inter", system-ui, -apple-system, "Segoe UI", Roboto, sans-serif',
                fontWeight: '500',
                fontSize: '1rem'
              }
            }
          }
      }
    })
    app.use(ToastService)
    app.use(ConfirmationService)

    
    app.component('Button', Button)
    app.component('InputText', InputText)
    app.component('DataTable', DataTable)
    app.component('Column', Column)
    app.component('Card', Card)
    app.component('Dialog', Dialog)
    app.component('Toast', Toast)
    app.component('ConfirmDialog', ConfirmDialog)
    app.component('MultiSelect', MultiSelect)
    app.component('FloatLabel', FloatLabel)
    app.component('Rating', Rating)
    app.component('Tag', Tag)
    app.component('ProgressSpinner', ProgressSpinner)
    app.component('Avatar', Avatar)
    app.component('Badge', Badge)
    app.component('Paginator', Paginator)
    app.component('Image', Image)
    app.component('Menubar', Menubar)
    app.component('Dock', Dock)
    app.component('Password', Password);
    app.component('Message', Message);
  }
}