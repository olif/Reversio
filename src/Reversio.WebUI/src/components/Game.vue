<template lang="html">
  <div class="game ui container">
    <div class="ui middle aligned grid">
      <div class="ui list game-status">
        <div class="item">
          <i class="large square icon"></i>
          <div class="content">
            <div class="header">
              <span v-if="blackPlayer !== null">
                {{blackPlayer.name}} [{{blackPlayer.score}}]
            </div>
          </div>
        </div>
        <div class="item">
          <i class="large square outline icon"></i>
          <div class="content">
            <div class="header">
              <span v-if="whitePlayer !== null">
                {{whitePlayer.name}} [{{whitePlayer.score}}]
              </span>
            </div>
          </div>
        </div>
        <div class="item">
          <div class="content">
            <button v-on:click="showModal()">Show modal</button>
          </div>
        </div>
      </div>  
      <table class="game-board" v-bind:class="getBoardColor">
        <tr v-for="(col, i) in board">
          <td v-for="(row, j) in col" @click="makeMove(i, j)" v-on:mouseover="hover(i, j, row, $event)" v-on:mouseleave="leave(i, j, $event)">
            <disc :discTypeNr="row"></disc>
          </td>
        </tr>
      </table>
    </div>
    
    <div class="ui basic modal">
      <div class="header">
        You won!
      </div>
      <div class="content">
        <p v-if="blackPlayer !== null && whitePlayer !== null">{{blackPlayer.score}} - {{whitePlayer.score}}</p>
        <p>Do you want to play a rematch?</p>
      </div>
      <div class="actions">
        <div class="ui deny red basic inverted button">
          <i class="remove icon"></i>
          No
        </div>
        <div class="ui ok green basic inverted button" v-on:click="inviteToRematch()">
          <i class="checkmark icon"></i>
          Yes
        </div>
      </div>
    </div>
  </template>

<script>
  import Disc from './Disc'
  import $ from 'jquery'

  export default {
    methods: {
      makeMove: function (i, j) {
        this.$store.dispatch('MAKE_MOVE', {y: i, x: j})
      },
      hover: function (i, j, color, e) {
        if (checkIfValidMove(this.$store.state.activeGame.currentState, [i, j], this.$store.getters.currentDiscColor) && this.isPlayersTurn()) {
          let elem = e.target
          elem.classList.add('hover')
        }
      },
      inviteToRematch: function () {
        this.$store.dispatch('INVITE_PLAYER', this.$store.getters.opponent)
      },
      isPlayersTurn: function () {
        return this.$store.state.activeGame.discOfNextMove.color === this.$store.getters.currentDiscColor
      },
      leave: function (i, j, e) {
        let elem = e.target
        elem.classList.remove('hover')
      },
      showModal: function () {
        $('.modal')
          .modal('setting', 'closable', false)
          .modal('show')
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
        console.log(this.$store.getters.currentDiscColor)
        let boardColor = this.$store.getters.currentDiscColor === -1 ? 'black-player-table' : 'white-player-table'
        console.log(boardColor)
        return boardColor
      }
    },
    components: {
      'disc': Disc
    },
    beforeMount () {
      if (this.$store.state.signedInUser === null) {
        this.$store.dispatch('LOAD_USER')
      }
      if (this.$store.state.activeGame.gameId === '') {
        let gameId = this.$router.currentRoute.params.id
        this.$store.dispatch('LOAD_GAME', gameId)
      }
    }
  }

  let directions = [
    [0, -1],
    [1, -1],
    [1, 0],
    [1, 1],
    [0, 1],
    [-1, 1],
    [-1, 0],
    [-1, -1]]

  function checkIfValidMove (board, position, color) {
    let isCandidate = false

    for (let i = 0; i < 8; i++) {
      let xPos = position[0]
      let yPos = position[1]
      let xStep = directions[i][0]
      let yStep = directions[i][1]

      for (let j = 0; j < 8; j++) {
        xPos += xStep
        yPos -= yStep

        if (xPos >= 8 || xPos < 0 || yPos >= 8 || yPos < 0 || board[xPos][yPos] === 0) {
          isCandidate = false
          break
        }

        if (board[xPos][yPos] === color) {
          break
        }

        isCandidate = true 
      }

      if (isCandidate === true) {
        return true
      }
    }

    return false
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

                                                         &.black-player-table td.hover .disc.empty {
                                                           background-color: rgba(0, 0, 0, 0.4);
                                                           box-shadow: inset 0px 0px 0px 2px rgba(255,255,255,0.2);
                                                         }

                                                         &.white-player-table td.hover .disc.empty {
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
