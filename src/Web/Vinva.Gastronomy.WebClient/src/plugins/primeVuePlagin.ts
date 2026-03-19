import { type App } from 'vue'
import PrimeVue from 'primevue/config'
import ToastService from 'primevue/toastservice'
import ConfirmationService from 'primevue/confirmationservice'

import Button from 'primevue/button'
import InputText from 'primevue/inputtext'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import Card from 'primevue/card'
import Dialog from 'primevue/dialog'
import Toast from 'primevue/toast'
import ConfirmDialog from 'primevue/confirmdialog'
import Dropdown from 'primevue/dropdown'
import MultiSelect from 'primevue/multiselect'
import Rating from 'primevue/rating'
import Tag from 'primevue/tag'
import ProgressSpinner from 'primevue/progressspinner'
import Avatar from 'primevue/avatar'
import Badge from 'primevue/badge'
import Paginator from 'primevue/paginator'
import Sidebar from 'primevue/sidebar'

export default {
  install(app: App) {
    app.use(PrimeVue, { ripple: true })
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
    app.component('Dropdown', Dropdown)
    app.component('MultiSelect', MultiSelect)
    app.component('Rating', Rating)
    app.component('Tag', Tag)
    app.component('ProgressSpinner', ProgressSpinner)
    app.component('Avatar', Avatar)
    app.component('Badge', Badge)
    app.component('Paginator', Paginator)
    app.component('Sidebar', Sidebar)
  }
}