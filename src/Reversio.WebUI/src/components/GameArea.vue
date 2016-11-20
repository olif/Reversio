<template>
<div class="gamearea-page ui container">
   <h1>Games</h1>
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
   computed: {
     activeGames: function () {
       return this.$store.state.activeGames
     }
   },
   beforeMount () {
     this.$store.dispatch('LOAD_GAMES')
   }
 }
</script>

<style lang="scss">
</style>
