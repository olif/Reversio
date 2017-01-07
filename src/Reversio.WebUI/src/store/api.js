/* global URLSearchParams */
import axios from 'axios'
import jscookie from 'js-cookie'

axios.defaults.baseURL = 'http://localhost:5000/api'

export default class Api {

  constructor () {
    let token = jscookie.get('access_token')
    if (token !== null) {
      this.accessToken = token
      this._setToken(token)
    } else {
      this.accessToken = null
    }
  }

  invitationResponse (invitee, acceptChallange) {
    let obj = {
      invitee: invitee,
      acceptChallange: acceptChallange
    }
    return axios.post('invitationresponse', obj)
  }

  createNewGame () {
    return axios.post('games/new')
  }

  signin (userName) {
    return axios.post('/signin', {
      userName: userName
    })
  }

  signinAsGuest () {
    let params = new URLSearchParams()
    params.append('username', 'guest')
    params.append('password', 'guest')

    return new Promise((resolve, reject) => {
      axios.post('/token', params)
        .then((response) => {
          let accessToken = response.data.access_token
          this._setToken(accessToken)
          resolve(accessToken)
        })
        .catch(() => {
          reject()
        })
    })
  }

  getUserInfo () {
    return axios.get('user')
  }

  /**
   * Invites another player to a game
   *
   * @param {Object} opponent
   * @returns {Promise}
   **/
  invitePlayer (opponent) {
    return axios.post('invite', opponent)
  }

  joinGame (game) {
    let uri = `${game.gameId}/join`
    return axios.post(uri)
  }

  _setToken (token) {
    axios.defaults.headers.common['Authorization'] = `Bearer ${token}`
  }
  
  loadGames () {
    return axios.get('active')
  }

  loadGame (gameId) {
    return axios.get(`games/${gameId}`)
  }
  
  loadPlayers () {
    return axios.get('players')
  }

  loadUser () {
    return axios.get('user')
  }
  
  makeMove (gameId, move) {
    let uri = `${gameId}/move`
    return axios.post(uri, move)
  }
}
