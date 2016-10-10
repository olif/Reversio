import {GameServer} from './gameserver.js';

export class GamesTable {
    constructor(createNewGameCallback) {
        this.element = this.createElement();
        this.createNewGame = createNewGameCallback;
    }

    createElement() {
        let html = `<div class="games-page">
                        <table class="game-table"></html>
                        </table>
                        <button type="button" class="new-game-btn">Create new game</button>
                    </div>`;

        $('body').on('click', '.new-game-btn', (e) => this.createNewGame(e))
        return $(html);
    }

    updateTable(games) {
        let html = '';
        let table = $(this.element).find('table');

        table.find('tr').remove();

        for(let game of games) {
            html += `<tr><td>${game.gameId}</td></tr>`
        }
        
        table.append($(html));
    }

    appendToElement(elem) {
        elem.append(this.element);
    }
}