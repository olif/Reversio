import {Board} from './board.js';

export class GamePage {
    constructor(gameserver) {
        this.gameserver = gameserver;
        this.gameserver.addGameStateListener((gameState) => this.onGameStateChangedHandler(gameState));
        this.board = new Board(this.gameserver.gameState, (pos) => this.onMoveHandler(pos));
        let html = `
            <div class="game-page">
            </div>
        `;
        this.element = $(html);
        this.board.appendToElement(this.element);
    }
    
    onGameStateChangedHandler(gameState) {
        if(gameState.discsFlipped !== undefined) {
            this.board.update(gameState);
        } 
    }

    onMoveHandler(pos) {
        this.gameserver.makeMove(pos);
    }

    appendToElement(elem) {
        elem.append(this.element);
    }
}