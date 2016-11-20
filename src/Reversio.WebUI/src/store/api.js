/* global URLSearchParams */
import axios from 'axios'

axios.defaults.baseURL = 'http://localhost:53274/api'

export default class Api {

  constructor () {
    this.accessToken = null
  }

  signin (userName) {
    return axios.post('/games/signin', {
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

  _setToken (token) {
    axios.defaults.headers.common['Authorization'] = `Bearer ${token}`
  }

  loadGames () {
    return axios.get('games/active')
  }
}
