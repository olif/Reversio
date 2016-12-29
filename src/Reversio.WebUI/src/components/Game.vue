import Disc from './Disc'

<template>
  <div class="game ui container">
    <div class="ui middle aligned grid">
    <div class="ui list game-status">
      <div class="item">
        <i class="large square icon"></i>
        <div class="content">
          <div class="header">
            {{blackPlayer.name}} [{{blackPlayer.score}}]
          </div>
        </div>
      </div>
      <div class="item">
        <i class="large square outline icon"></i>
        <div class="content">
          <div class="header">
            {{whitePlayer.name}} [{{whitePlayer.score}}]
          </div>
        </div>
      </div>
    </div>
    <table class="game-board" v-bind:class="getBoardColor">
      <tr v-for="(col, i) in board">
        <td v-for="(row, j) in col" @click="makeMove(i, j)">
          <disc :discTypeNr="row"></disc>
        </td>
      </tr>
    </table>
  </div>
</template>

<script>
import Disc from './Disc'

export default {
  methods: {
    makeMove: function (i, j) {
      this.$store.dispatch('MAKE_MOVE', {y: i, x: j})
    }
  },
  computed: {
    board: function () {
      return this.$store.state.activeGame.currentState
    },
    blackPlayer: function () {
      return this.$store.state.activeGame.blackPlayerStatus
    },
    whitePlayer: function () {
      return this.$store.state.activeGame.whitePlayerStatus
    },
    getBoardColor: function () {
      console.log(this.$store.state.activeDisc)
      return this.$store.state.activeDisc === -1 ? 'black-player-table' : 'white-player-table'
    }
  },
  components: {
    'disc': Disc
  }
}
</script>

<style lang="scss" scoped>
  .game {
    margin-top: 100px;
  }

  .game-status {
    position: absolute;
  }

  .game-board {
    background-color: #339966;
    width: 600px;
    height: 600px;
    margin: 0 auto;
    border-collapse: collapse;
    border: 1px solid #206040;

    &.black-player-table td:hover .disc.empty {
      background-color: rgba(0, 0, 0, 0.4);
      box-shadow: inset 0px 0px 0px 2px rgba(255,255,255,0.2);
    }

    &.white-player-table td:hover .disc.empty {
      background-color: rgba(255, 255, 255, 0.4);
      box-shadow: inset 0px 0px 0px 2px rgba(255,255,255,0.2);
    }

    td {
      text-align: center;
      vertical-align: middle;
      border: 1px solid #206040
    }
  }
</style>