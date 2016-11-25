import Vue from 'vue'
import Vuex from 'vuex'
import router from '../router'
import Api from './api'
import SocketHandler from './socket'

Vue.use(Vuex)

const api = new Api()

const store = new Vuex.Store({
  state: {
    signedInUser: null,
    currentState: null,
    activeGames: [],
    activeGame: {
      disc: 0,
      state: [],
      gameId: null
    }
  },

  actions: {

    SIGN_IN: function ({ commit }, username) {
      return new Promise((resolve, reject) => {
        api.signin(username)
          .then((response) => {
            commit('SET_USER', response.data)
            socketHandler.connect(response.data.id)
            resolve()
          })
          .catch((error) => {
            console.log('det sket sig')
            reject(error)
          })
      })
    },

    SIGN_IN_AS_GUEST: function ({ commit }) {
      return new Promise((resolve, reject) => {
        api.signinAsGuest()
          .then((token) => {
            console.log(token)
            document.cookie = `access_token=${token}`
            socketHandler.connect(token)
            resolve()
          })
          .catch((error) => {
            console.log('det sket sig')
            reject(error)
          })
      })
    },

    LOAD_GAMES: function ({commit}) {
      api.loadGames()
        .then((games) => {
          commit('SET_ACTIVE_GAMES', games.data)
        })
    },

    WAIT_FOR_PLAYER: function ({commit}) {
      socketHandler.waitForOpponent()
      commit('WAIT_FOR_PLAYER')
    },

    START_GAME: function ({commit}, gameStartedState) {
      commit('START_GAME', gameStartedState)
      console.log(gameStartedState)
      router.push({name: 'game', params: {id: gameStartedState.currentState.gameId}})
    },

    MAKE_MOVE: function ({commit, state}, move) {
      socketHandler.makeMove(state.activeGame.gameId, state.signedInUser, move)
    },

    UPDATE_GAME_STATE: function ({commit}, gameState) {
      commit('PLACE_DISC', gameState.currentState.lastValidMove)
      for (let disc of gameState.currentState.discsFlipped) {
        commit('UPDATE_POSITION', disc)
      }
    }
  },

  mutations: {

    SET_USER: (state, user) => {
      state.signedInUser = user
    },

    WAIT_FOR_PLAYER: (state) => {
      state.currentState = 'WAITING_FOR_PLAYER'
    },

    SET_ACTIVE_GAMES: (state, games) => {
      state.activeGames = games
    },

    START_GAME: (state, gameStartedState) => {
      state.activeGame.discColor = gameStartedState.playerAssignedDisc
      state.activeGame.state = gameStartedState.currentState
      state.activeGame.gameId = gameStartedState.currentState.gameId
    },

    PLACE_DISC: (state, disc) => {
      let i = disc.position.y
      let j = disc.position.x
      let discColor = disc.disc.color
      let row = state.activeGame.state.currentState[i]
      row.splice(j, 1, discColor)
    },

    UPDATE_POSITION: (state, position) => {
      let i = position.y
      let j = position.x
      let row = state.activeGame.state.currentState[i]
      let disc = row[j]
      row.splice(j, 1, disc * -1)
    },

    UPDATE_GAME_STATE: (state, gameState) => {
      state.activeGame.state = gameState
    }
  }
})

const socketHandler = new SocketHandler(store)
export default store
