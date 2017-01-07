import Vue from 'vue'
import Vuex from 'vuex'
import Api from './api'
import SocketHandler from './socket'
import States from './states'
import Router from '../router.js'

Vue.use(Vuex)

const store = new Vuex.Store({
  state: {
    signedInUser: null,
    currentState: States.UNDEFINED,
    inviter: null,
    activeGames: [],
    opponents: [],
    declinedInvitations: [],
    activeGame: {
      gameId: '',
      lastValidMove: null,
      discsFlipped: [],
      discOfNextMove: null,
      isGameFinished: false,
      currentState: [],
      blackPlayerStatus: null,
      whitePlayerStatus: null
    },
    activeDisc: 0
  },

  actions: {

    INVITATION_RESPONSE: function ({commit, state}, acceptChallange) {
      return api.invitationResponse(state.inviter, acceptChallange).then(() => {
        console.log('clearing inviter')
        commit('CLEAR_INVITER')
        commit('SET_STATE', States.UNDEFINED)
      })
    },

    GAME_INVITATION_RECEIVED: function ({commit, state}, inviter) {
      commit('SET_STATE', States.RECEIVED_INVITATION)
      commit('SET_INVITER', inviter)
    },

    GAME_INVITATION_DECLINED: function ({commit}, response) {
      commit('ADD_DECLINED_INVITATION', response)
    },

    INVITE_PLAYER: function ({commit}, opponent) {
      return api.invitePlayer({opponent: opponent})
        .then(() => commit('SET_STATE', States.WAITING_ON_INVITATION_RESPONSE))
    },

    JOIN_GAME: function ({commit}, game) {
      return api.joinGame(game)
        .then((response) => {
          commit('START_GAME', response.data)
          commit('SET_STATE', States.PLAYING)
        })
    },

    LOAD_GAME: function ({commit}, gameId) {
      return api.loadGame(gameId)
        .then((response) => {
          commit('SET_GAME', response.data)
        })
    },

    LOAD_GAMES: function ({commit}) {
      return api.loadGames()
        .then((games) => {
          commit('SET_ACTIVE_GAMES', games.data)
        })
    },

    LOAD_PLAYERS: function ({commit}) {
      return api.loadPlayers()
        .then((players) => {
          commit('SET_PLAYERS', players.data)
        })
    },

    LOAD_USER: function ({commit}) {
      return new Promise((resolve, reject) => {
        api.loadUser()
         .then((response) => {
           commit('SET_USER', response.data)
           console.log('resolved')
           resolve()
         })
      })
    },

    MAKE_MOVE: function ({commit, state}, move) {
      return api.makeMove(state.activeGame.gameId, move)
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
            api.getUserInfo()
              .then((response) => {
                commit('SET_USER', response.data)
                resolve()
              })
            document.cookie = `access_token=${token}`
            socketHandler.connect(token)
          })
          .catch((error) => {
            console.log('det sket sig')
            reject(error)
          })
      })
    },

    // Used when the game was started by the server
    START_GAME: function ({commit}, game) {
      commit('START_GAME', game.currentState)
      commit('SET_STATE', States.PLAYING)
      Router.push({ name: 'game', params: { id: game.gameId } })
    },

    START_NEW_GAME: function ({commit, state}) {
      return new Promise((resolve, reject) => {
        api.createNewGame().then((game) => {
          commit('START_GAME', game.data)
          commit('SET_STATE', States.PLAYING)
          resolve(game.data)
        })
        .catch((error) => {
          reject(error)
        })
      })
    },

    UPDATE_GAME_STATE: function ({commit}, gameState) {
      commit('UPDATE_GAME_STATE', gameState.currentState)
    }
  },

  mutations: {

    ADD_DECLINED_INVITATION: (state, declined) => {
      state.declinedInvitations.push(declined)
    },

    CLEAR_INVITER: (state) => {
      state.inviter = null
    },

    SET_USER: (state, user) => {
      state.signedInUser = user
    },

    SET_ACTIVE_GAMES: (state, games) => {
      state.activeGames = games
    },

    SET_GAME: (state, gameState) => {
      state.activeGame = gameState
    },

    SET_INVITER: (state, inviter) => {
      state.inviter = inviter
    },

    SET_PLAYERS: (state, players) => {
      state.opponents = players
    },

    START_GAME: (state, gameStarted) => {
      console.log('game started')
      console.log(gameStarted)
      state.activeGame = gameStarted
      if (gameStarted.blackPlayerStatus.name === state.signedInUser) {
        state.activeDisc = -1
      } else if (gameStarted.whitePlayerStatus.name === state.signedInUser) {
        state.activeDisc = 1
      } else {
        throw new Error('Could not determine disc color')
      }
    },

    SET_STATE: (state, newState) => {
      state.currentState = newState
    },

    UPDATE_GAME_STATE: (state, gameState) => {
      // Place disc
      let move = gameState.lastValidMove
      if (move !== undefined) {
        let i = move.position.y
        let j = move.position.x
        let discColor = move.disc.color
        let row = state.activeGame.currentState[i]
        row.splice(j, 1, discColor)
      }

      for (let flippedDisc of gameState.discsFlipped) {
        let i = flippedDisc.y
        let j = flippedDisc.x
        let row = state.activeGame.currentState[i]
        let disc = row[j]
        row.splice(j, 1, disc * -1)
      }

      // Update next move
      console.log(gameState.discOfNextMove)
      state.activeGame.discOfNextMove = gameState.discOfNextMove

      // Update user info
      state.activeGame.blackPlayerStatus.name = gameState.blackPlayerStatus.name
      state.activeGame.blackPlayerStatus.score = gameState.blackPlayerStatus.score
      state.activeGame.whitePlayerStatus.name = gameState.whitePlayerStatus.name
      state.activeGame.whitePlayerStatus.score = gameState.whitePlayerStatus.score
    }
  },  

  getters: {

    currentDiscColor: state => {
      if (state.signedInUser !== null && state.activeGame.blackPlayerStatus !== null &&
            state.activeGame.blackPlayerStatus.name === state.signedInUser) {
        return -1
      } else if (state.signedInUser !== null && state.activeGame.whitePlayerStatus &&
          state.activeGame.whitePlayerStatus.name === state.signedInUser) {
        return 1
      } else {
        return 0
      }
    },

    currentState: state => state,

    opponent: state => {
      if (state.currentState === States.PLAYING) {
        if (state.signedInUser === state.activeGame.blackPlayerStatus.name) {
          return {name: state.activeGame.whitePlayerStatus.name}
        } else {
          return {name: state.activeGame.blackPlayerStatus.name}
        }
      }
    },

    opponents: state => {
      let opponents = []
      for (let opponent of state.opponents) {
        let op = opponent
        console.log('declined invitations')
        console.log(state.declinedInvitations)
        for (let dec of state.declinedInvitations) {
          console.log('--------------')
          console.log(opponent.name)
          console.log(dec.invitee.name)
          if (opponent.name === dec.invitee.name) {
            console.log('has declined')
            op.hasDeclined = true
          } else {
            op.hasDeclined = false
          }
        }

        opponents.push(op)
      }

      return opponents
    }

  }
})

const api = new Api()
const socketHandler = new SocketHandler(store)
// If the api has an accesstoken the user is already signed in
// -> open websocket and load signed in user
if (api.accessToken) {
  socketHandler.connect(api.accessToken)
  store.dispatch('LOAD_USER')
}
export default store
