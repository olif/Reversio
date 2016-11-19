/* global WebSocket */
export default class GameSocketHandler {

  constructor (store) {
    this.store = store
  }

  connect (userId) {
    this.userId = userId
    this.socket = new WebSocket(`ws://localhost:53274?sessionId=${userId}`)
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
        console.log(msg.payload)
        this.store.dispatch('UPDATE_GAME_STATE', msg.payload)
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
        bystander: {
          id: this.userId
        }
      }
    }
    this.socket.send(JSON.stringify(msg))
  }
}
