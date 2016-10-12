import {Board} from './board.js';

export class GamePage {
    constructor(gameserver) {
        this.gameserver = gameserver;
        this.board = new Board(this.gameserver.gamestate);
        let html = `
            <div class="game-page">
            </div>
        `;
        this.element = $(html);
        this.board.appendToElement(this.element);
    }

    appendToElement(elem) {
        elem.append(this.element);
    }
}