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
          preset: {
            ...Aura,
            semantic: {
              ...Aura.semantic,
              primary: {
                50: '#f2f6f2',
                100: '#e0e8e0',
                200: '#c1d1c1',
                300: '#a2baa2',
                400: '#83a383',
                500: '#709170',
                600: '#5a745a',
                700: '#435743',
                800: '#2d3a2d',
                900: '#161d16',
                950: '#0b0e0b'
              },
              colorScheme: {
                light: {
                  primary: {
                    color: '{primary.500}',
                    contrastColor: '#ffffff',
                    hoverColor: '{primary.600}',
                    activeColor: '{primary.700}'
                  }
                },
                dark: {
                  primary: {
                    color: '{primary.400}',
                    contrastColor: '#ffffff',
                    hoverColor: '{primary.300}',
                    activeColor: '{primary.200}'
                  }
                }
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