import Vue from 'vue'
import VueRouter from 'vue-router'
import Signin from './components/Signin'
import Hello from './components/Hello'
import GameArea from './components/GameArea'
import Game from './components/Game'

Vue.use(VueRouter)

const routes = [
  { path: '/' },
  { path: '/signin', component: Signin },
  { path: '/gamearea', component: GameArea },
  { path: '/:id/game', component: Game, name: 'game' },
  { path: '/hello', component: Hello }
]

const router = new VueRouter({
  routes
})

export default router
