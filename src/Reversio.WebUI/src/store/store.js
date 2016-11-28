import Vue from 'vue'
import Vuex from 'vuex'
import router from '../router'
import Api from './api'
import SocketHandler from './socket'

Vue.use(Vuex)

const api = new Api()

const states = {
  UNDEFINED: 'UNDEFINED',
  WAITING_FOR_PLAYER: 'WAITING_FOR_PLAYER',
  WAITING_ON_INVITATION_RESPONSE: 'WAITING_ON_INVITATION_RESPONSE',
  PLAYING: 'PLAYING'
}

const store = new Vuex.Store({
  state: {
    signedInUser: null,
    currentState: states.UNDEFINED,
    activeGames: [],
    opponents: [],
    activeGame: {
      disc: 0,
      state: [],
      gameId: null
    }
  },

  actions: {

    INVITE_PLAYER: function ({commit}, opponent) {
      return api.invitePlayer(opponent)
        .then(() => commit('SET_STATE', states.WAITING_ON_INVITATION_RESPONSE))
    },

    LOAD_GAMES: function ({commit}) {
      api.loadGames()
        .then((games) => {
          commit('SET_ACTIVE_GAMES', games.data)
        })
    },
    
    LOAD_PLAYERS: function ({commit}) {
      api.loadPlayers()
        .then((players) => {
          commit('SET_PLAYERS', players.data)
        })
    },

    MAKE_MOVE: function ({commit, state}, move) {
      socketHandler.makeMove(state.activeGame.gameId, state.signedInUser, move)
    },

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

    START_GAME: function ({commit}, gameStartedState) {
      commit('START_GAME', gameStartedState)
      console.log(gameStartedState)
      router.push({name: 'game', params: {id: gameStartedState.currentState.gameId}})
    },

    UPDATE_GAME_STATE: function ({commit}, gameState) {
      commit('PLACE_DISC', gameState.currentState.lastValidMove)
      for (let disc of gameState.currentState.discsFlipped) {
        commit('UPDATE_POSITION', disc)
      }
    },

    WAIT_FOR_PLAYER: function ({commit}) {
      socketHandler.waitForOpponent()
      commit('SET_STATE', states.WAITING_FOR_PLAYER)
    }
  },

  mutations: {

    SET_USER: (state, user) => {
      state.signedInUser = user
    },

    SET_ACTIVE_GAMES: (state, games) => {
      state.activeGames = games
    },

    SET_PLAYERS: (state, players) => {
      state.opponents = players
    },

    START_GAME: (state, gameStartedState) => {
      state.activeGame.discColor = gameStartedState.playerAssignedDisc
      state.activeGame.state = gameStartedState.currentState
      state.activeGame.gameId = gameStartedState.currentState.gameId
      state.currentState = states.PLAYING
    },

    SET_STATE: (state, newState) => {
      state.currentState = newState
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
  },
  
  getters: {
    currentState: state => state.currentState
  }
})

const socketHandler = new SocketHandler(store)
export default store
