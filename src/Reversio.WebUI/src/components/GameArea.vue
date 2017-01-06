<template lang="html">
  <div class="gamearea-page ui container">
    <h1>Games</h1>
    <div class="ui grid">
      <div class="right aligned three wide column ">
        <h3>Controls</h3>
        <div class="ui compact vertical labeled icon menu">
          <a class="item" v-on:click.prevent="startNewGame">
            <i class="gamepad icon"></i>
            Start new game
          </a>
        </div>
      </div>
      
      <div class="eight wide column">
        <h3>Current games</h3>
        <div class="ui relaxed divided list">
          
          <div v-if="activeGames.length > 0">
            <div class="item" v-for="game in activeGames">
              <div class="right floated content">
                <button type="button" class="ui primary button" v-on:click="join(game, $event)" v-if="game.gameState.state == 'WaitingForOpponent'">Join</button>
                <button type="button" class="ui primary button">Watch</button>
              </div>
              <i class="large circle middle aligned icon"></i>
              <div class="content">
                <a class="header">
                  {{game.blackPlayerStatus.name}} [{{game.blackPlayerStatus.score}}] 
                  - 
                  {{game.whitePlayerStatus.name}} [{{game.whitePlayerStatus.score}}]
                </a>
                <div class="description">
                </div>
              </div>
            </div>
          </div>
          <div v-if="activeGames.length == 0">
            <p>No active games</p>
          </div>
        </div>
      </div>
      <div class="five wide column">
        <h3>Players</h3>
        <div class="ui middle aligned divided list">
          <div class="item" v-for="player in registeredPlayers">
            <div class="right floated content">
              <button class="ui toggle button" v-on:click="invite(player,  $event)">Invite</button>
            </div>
            <i class="large user icon"></i>
            <div class="content">
              <a class="header">{{ player.name }}</a>
              <div class="description">
                description
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <div class="ui active dimmer" v-if="invitationReceived">
      <div class="ui basic active modal">
        <div class="header">
         You got a game invitation 
        </div>
        <div class="content">
          <p>You have been invited to play a game with: <strong>{{invitee.name}}</strong></p>
        </div>
        <div class="actions">
          <div class="ui deny red basic inverted button">
            <i class="remove icon"></i>
            No
          </div>
          <div class="ui ok green basic inverted button" v-on:click="acceptInvitation()">
            <i class="checkmark icon"></i>
            Yes
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
  import $ from 'jquery'
  export default {
    data: function () {
      return {
        username: ''
      }
    },
    methods: {
      test (e) {
        console.log($('.ui.basic.modal'))
      },
      waitForPlayer (e) {
        console.log('waiting for player')
        this.$store.dispatch('WAIT_FOR_PLAYER')
      },
      startNewGame (e) {
        this.$store.dispatch('START_NEW_GAME')
          .then(() => {
            console.log(this.$store.state)
            this.$router.push({ name: 'game', params: { id: this.$store.state.activeGame.gameId } })
          })
      },
      join (game, e) {
        this.$store.dispatch('JOIN_GAME', game)
          .then(() => {
            this.$router.push({ name: 'game', params: { id: this.$store.state.activeGame.gameId } })
          })
      },
      invite (player, e) {
        console.log(player)
        this.$store.dispatch('INVITE_PLAYER', player)
      }
    },
    computed: {
      activeGames: function () {
        return this.$store.state.activeGames
      },
      registeredPlayers: function () {
        return this.$store.state.opponents
      },
      invitationReceived: function () {
        console.log('checking invitation')
        console.log(this.$store.state.currentState)
        let result = this.$store.state.currentState === 'RECEIVED_INVITATION'
        console.log(result)
        return result
      },
      isWaitingForOpponent: function () {
        console.log('checking state')
        console.log(this.$store.state.currentState)
        return this.$store.state.currentState === 'WAITING_FOR_PLAYER'
      },
      invitee: function () {
        return this.$store.state.invitee
      }
    },
    beforeMount () {
      this.$store.dispatch('LOAD_GAMES')
      this.$store.dispatch('LOAD_PLAYERS')
    }
  }
</script>

<style lang="scss">

</style>
