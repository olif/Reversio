import {GameServer} from './gameserver.js';

export class GamesTable {
    constructor() {
        this.element = this.createElement();
    }

    createElement() {
        let html = '<table class="game-table"></html>'
        html += '</table>';
        return $(html);
    }

    updateTable(games) {
        games = [{gameId: 1}];
        this.element.find('tr').remove();
        let html = '';
        for(let game of games) {
            html += `<tr><td>${game.gameId}</td></tr>`
        }
        this.element.append($(html));
    }

    appendToElement(elem) {
        elem.append(this.element);
    }
}