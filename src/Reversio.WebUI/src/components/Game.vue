import Disc from './Disc'

<template>
  <div class="game ui container">
    <table class="game-area">
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
      return this.$store.state.activeGame.state.currentState
    }
  },
  components: {
    'disc': Disc
  }
}
</script>

<style lang="scss" scoped>
  .game-area {
    background-color: #339966;
    width: 600px;
    height: 600px;
    border-collapse: collapse;
    border: 1px solid #206040;

    td {
      text-align: center;
      vertical-align: middle;
      border: 1px solid #206040
    }
  }
</style>