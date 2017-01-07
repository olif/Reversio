<template lang="html">
  <div class="gamearea-page ui container">

    <h1 class="ui center aligned header">Games</h1>

    <div class="ui relaxed grid">
      <div class="right aligned two wide column ">
        <h3>Controls</h3>
        <div class="ui compact vertical labeled icon menu">
          <a class="teal item" v-on:click.prevent="startNewGame">
            <i class="gamepad icon"></i>
            Start new game
          </a>
        </div>
      </div>
      
      <div class="ten wide column">
        <h3>Current games</h3>
        <div class="ui middle aligned divided list games" v-if="activeGames.length > 0">
          <div class="item" v-for="game in activeGames">
            <div class="right floated content">
              <button type="button" class="ui teal button" v-on:click="join(game, $event)" v-if="game.gameState.state == 'WaitingForOpponent'">Join</button>
              <button type="button" class="ui teal button">Watch</button>
            </div>
            <i class="large circle icon image"></i>
            <div class="content">
              <div class="header">
                {{game.blackPlayerStatus.name}} [{{game.blackPlayerStatus.score}}] 
                - 
                {{game.whitePlayerStatus.name}} [{{game.whitePlayerStatus.score}}]
              </div>
            </div>
          </div>
          <div v-if="activeGames.length == 0">
            <p>No active games</p>
          </div>
        </div>
      </div>

      <div class="four wide grey column">
        <h3>Players</h3>
        <div class="ui middle aligned divided list">
          <div class="item" v-for="player in registeredPlayers">
            <div class="right floated content">
              <button class="ui toggle teal button" :disabled="player.hasDeclined" v-on:click="invite(player, $event)">Invite</button>
            </div>
            <i class="large user icon"></i>
            <div class="content">
              <a class="header">{{ player.name }}</a>
              <div class="description">
                <span v-if="player.hasDeclined">Has declined invitation</span>
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
          <p>You have been invited to play a game with: <strong>{{inviter.name}}</strong></p>
        </div>
        <div class="actions">
          <div class="ui deny red basic inverted button" v-on:click="declineInvitation()">
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
  export default {
    data: function () {
      return {
        username: ''
      }
    },
    methods: {
      acceptInvitation () {
        this.$store.dispatch('INVITATION_RESPONSE', true)
      },
      declineInvitation () {
        this.$store.dispatch('INVITATION_RESPONSE', false)
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
        let opponents = this.$store.getters.opponents
        console.log(opponents)
        return opponents
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
      inviter: function () {
        return this.$store.state.inviter
      }
    },
    beforeMount () {
      this.$store.dispatch('LOAD_GAMES')
      this.$store.dispatch('LOAD_PLAYERS')
    }
  }
  </script>

<style lang="scss" scoped>

  .gamearea-page {
      margin-top: 80px;
    }

  .list.games .item {
    padding: 20px 0;
  }

  .grey.column .content a.header {
    color: white!important;
  }

  .ui.header {
    margin-bottom: 40px;
  }

</style>
