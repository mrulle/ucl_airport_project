import { createRouter, createWebHistory } from 'vue-router'
import BookingView from '../views/BookingView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'booking',
      component: BookingView
    },
    {
      path: '/checkin',
      name: 'checkin',
      // route level code-splitting
      // this generates a separate chunk (About.[hash].js) for this route
      // which is lazy-loaded when the route is visited.
      component: () => import('../views/CheckinView.vue')
    },
    {
      path: '/booking/:flightId/book',
      name: 'details',
      component: () => import('../views/BookingDetailsView.vue'),
      props: true
    }
  ]
})

export default router
