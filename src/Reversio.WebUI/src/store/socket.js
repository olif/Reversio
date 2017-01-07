/* global WebSocket */
export default class GameSocketHandler {

  constructor (store) {
    this.store = store
  }

  connect (token) {
    this.token = token
    this.socket = new WebSocket(`ws://localhost:5000`)
    this.socket.onopen = (e) => console.log('ws connection open')
    this.socket.onmessage = (e) => this.dispatchMessage(e)
  }

  dispatchMessage (e) {
    let msg = JSON.parse(e.data)
    console.log(msg)

    switch (msg.messageType) {

      case 'reversio.event.gameStarted':
        console.log('game started from server')
        this.store.dispatch('START_GAME', msg.payload)
        break

      case 'reversio.event.gameStateChanged':
        this.store.dispatch('UPDATE_GAME_STATE', msg.payload)
        break

      case 'reversio.event.gameInvitation':
        this.store.dispatch('GAME_INVITATION_RECEIVED', msg.payload.inviter)
        break

      case 'reversio.event.gameInvitationDeclined':
        this.store.dispatch('GAME_INVITATION_DECLINED', msg.payload)
        break
    }
  }
}
