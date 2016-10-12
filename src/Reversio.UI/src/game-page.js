import {Board} from './board.js';

export class GamePage {
    constructor(gameserver) {
        this.gameserver = gameserver;
        this.board = new Board(this.gameserver.gamestate, (pos) => this.onMoveHandler(pos));
        let html = `
            <div class="game-page">
            </div>
        `;
        this.element = $(html);
        this.board.appendToElement(this.element);
    }

    onMoveHandler(pos) {
        console.log(this);
        console.log('on move handler');
        console.log(pos);
        this.gameserver.makeMove(pos);
    }

    appendToElement(elem) {
        elem.append(this.element);
    }
}