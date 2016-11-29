<template>
  <div class="gamearea-page ui container">
    <h1>Games</h1>
    <div class="ui grid">
      <div class="four wide column">
          <h3>Controls</h3>
          <div class="ui vertical buttons">
            <button class="ui button primary" v-on:click="waitForPlayer">Create new game</button>
            <button class="ui button primary" v-on:click="waitForPlayer">Wait for player</button>
          </div>
        <span v-if="isWaitingForOpponent">Waiting for an opponent</span>
      </div>
      
      <div class="eight wide column">
        <h3>Current games</h3>
        <div class="ui relaxed divided list">
          <div class="item" v-for="game in activeGames">
            <div class="right floated content">
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
      </div>
      <div class="four wide column">
        <h3>Players</h3>
        hello
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
    waitForPlayer (e) {
      console.log('waiting for player')
      this.$store.dispatch('WAIT_FOR_PLAYER')
    }
  },
  computed: {
    activeGames: function () {
      return [{
        gameId: '522a7ab2-c379-4cc7-887f-ea810039ec8d',
        lastValidMove: null,
        discsFlipped: null,
        discOfNextMove: {
          color: -1
        },
        isGameFinished: false,
        currentState: [],
        blackPlayerStatus: {
          name: 'guest-67379',
          score: 2
        },
        whitePlayerStatus: {
          name: 'guest-62894',
          score: 2
        }
      }, {
        gameId: '522a7ab2-c379-4cc7-887f-ea810039ec8d',
        lastValidMove: null,
        discsFlipped: null,
        discOfNextMove: {
          color: -1
        },
        isGameFinished: false,
        currentState: [],
        blackPlayerStatus: {
          name: 'guest-67379',
          score: 2
        },
        whitePlayerStatus: {
          name: 'guest-62894',
          score: 2
        }
      }]
    },
    isWaitingForOpponent: function () {
      console.log('checking state')
      console.log(this.$store.state.currentState)
      return this.$store.state.currentState === 'WAITING_FOR_PLAYER'
    }
  },
  beforeMount () {
    this.$store.dispatch('LOAD_GAMES')
  }
}
</script>

<style lang="scss">

</style>