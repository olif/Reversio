<template>
<div class="gamearea-page ui container">
   <h1>Games</h1>
   <button v-on:click="waitForPlayer">Wait for player</button>
   <span v-if="isWaitingForOpponent">Waiting for an opponent</span>
   <div class="ui relaxed divided list">
    <div class="item" v-for="game in activeGames">
      <i class="large github middle aligned icon"></i>
      <div class="content">
        <a class="header">{{game.gameId}}</a>

        <div class="description">
          {{game}}
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
     waitForPlayer (e) {
       console.log('waiting for player')
       this.$store.dispatch('WAIT_FOR_PLAYER')
     }
   },
   computed: {
     activeGames: function () {
       return this.$store.state.activeGames
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
