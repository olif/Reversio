import axios from 'axios'

axios.defaults.baseURL = 'http://localhost:53274/api'

export default class Api {

  signin (userName) {
    return axios.post('/games/signin', {
      userName: userName
    })
  }

  loadGames () {
    return axios.get('games/active')
  }
}
