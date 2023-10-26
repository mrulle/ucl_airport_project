import './assets/main.css'

import { createApp } from 'vue'
import App from './App.vue'
import router from './router'

// TODO:
// Load in url's through environment variables here
// inspo: https://github.com/AndersBjerregaard/cl23-exam-project/blob/main/frontend/src/main.js

const apiHost = import.meta.env.VITE_API_HOST

const app = createApp(App)

app.provide('api', apiHost)

app.use(router)

app.mount('#app')
