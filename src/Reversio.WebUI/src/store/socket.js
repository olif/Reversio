/* global WebSocket */
export default class GameSocketHandler {

  constructor (store) {
    this.store = store
  }

  connect (token) {
    this.token = token
    this.socket = new WebSocket(`ws://localhost:53274`)
    this.socket.onopen = (e) => console.log('ws connection open')
    this.socket.onmessage = (e) => this.dispatchMessage(e)
  }

  dispatchMessage (e) {
    let msg = JSON.parse(e.data)
    console.log(msg)

    switch (msg.messageType) {
      case 'reversio.event.gameStarted':
        console.log(msg.payload)
        this.store.dispatch('START_GAME', msg.payload)
        break

      case 'reversio.event.gameStateChanged':
        this.store.dispatch('UPDATE_GAME_STATE', msg.payload)
        break

      case 'reversio.event.gameInvitationDeclined':
        this.store.dispatch('GAME_INVITATION_DECLINED')
        break
    }
  }

  makeMove (gameId, user, position) {
    let msg = {
      messageType: 'reversio.event.move',
      payload: {
        gameId: gameId,
        bystander: user,
        position: position
      }
    }

    this.socket.send(JSON.stringify(msg))
  }

  waitForOpponent () {
    let msg = {
      messageType: 'reversio.event.startGameWithRandomPlayer',
      payload: {
        name: 'this.userIdtest'
      }
    }
    this.socket.send(JSON.stringify(msg))
  }
}
