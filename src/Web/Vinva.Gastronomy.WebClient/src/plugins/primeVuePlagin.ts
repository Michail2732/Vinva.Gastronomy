import { type App } from 'vue'
import PrimeVue from 'primevue/config'
import ToastService from 'primevue/toastservice'
import IconField from 'primevue/iconfield'
import InputIcon from 'primevue/inputicon'
import Toolbar from 'primevue/toolbar'
import ConfirmationService from 'primevue/confirmationservice'
import Aura from '@primeuix/themes/aura'
import Button from 'primevue/button'
import InputText from 'primevue/inputtext'
import DataTable from 'primevue/datatable'
import SelectButton from 'primevue/selectbutton'
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
import { definePreset } from '@primeuix/themes'

export default {
  install(app: App) {

    const preset = definePreset(Aura, 
      {
        semantic: {
          primary: {
                50: '#e6f3ec',
                100: '#cce2d4',
                200: '#99c5aa',
                300: '#66a87f',
                400: '#4d976c',
                500: '#3D8A60',     // ваш основной цвет
                600: '#367c56',
                700: '#2e6a48',
                800: '#26583b',
                900: '#1e462e',
                950: '#163522'
          },
           secondary: {
            50: '#f5f0ed',  // очень светлый
            100: '#e8dfd9',
            200: '#d1bfb3',
            300: '#ba9f8d',
            400: '#a37f67',
            500: '#74513D',  // ваш основной secondary цвет
            600: '#684937',
            700: '#5c4031',
            800: '#50372b',
            900: '#442e25',
            950: '#38251f'   // очень темный
        },
          colorScheme: {
            light: {
              primary: {
                color: '{primary.500}',
                contrastColor: '#ffffff',
                hoverColor: '{primary.600}',
                activeColor: '{primary.700}'
              },
              secondary: {
                  color: '{secondary.500}',
                  contrastColor: '#ffffff',
                  hoverColor: '{secondary.600}',
                  activeColor: '{secondary.700}'
              }
            }
          }
        }
      }
    )

    app.use(PrimeVue,
      {
        ripple: true,
        unstyled: false,
        theme: {
          preset: preset
        }
      }
    )
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
    app.component('SelectButton', SelectButton);
    app.component('IconField', IconField);
    app.component('InputIcon', InputIcon);
    app.component('Toolbar', Toolbar);

  }
}