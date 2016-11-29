/* global URLSearchParams */
import axios from 'axios'

axios.defaults.baseURL = 'http://localhost:53274/api'

export default class Api {

  constructor () {
    this.accessToken = null
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

  /**
   * Invites another player to a game
   *
   * @param {Object} opponent
   * @returns {Promise}
   **/
  invitePlayer (opponent) {
    return axios.post('invite', opponent)
  }

  _setToken (token) {
    axios.defaults.headers.common['Authorization'] = `Bearer ${token}`
  }
  
  loadGames () {
    return axios.get('active')
  }
  
  loadPlayers () {
    return axios.get('players')
  }
  
  makeMove (gameId, move) {
    let uri = `${gameId}/move`
    return axios.post(uri, move)
  }
  
  startRandomGame () {
    return axios.post('randomgame')
  }
}
