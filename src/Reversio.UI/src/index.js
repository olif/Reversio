import {GameServer} from './gameserver.js';

export class IndexPage {
    constructor(createNewGameCallback, joinGameCallback, createNewRandomGameCallback) {
        this.element = this.createElement();
        this.createNewGame = createNewGameCallback;
        this.createNewRandomGame = createNewRandomGameCallback;
        this.joinGame = joinGameCallback;
    }

    createElement() {
        let html = `<div class="games-page">
                        <table class="game-table"></html>
                        </table>
                        <button type="button" class="new-game-btn">Create new game</button>
                        <button type="button" class="new-random-game-btn">Create game with random player</button>
                    </div>`;

        $('body').on('click', '.new-game-btn', (e) => this.createNewGame(e))
        $('body').on('click', '.new-random-game-btn', (e) => this.createNewRandomGame(e))
        return $(html);
    }

    updateTable(games) {
        let html = '';
        let table = $(this.element).find('table');

        table.find('tr').remove();

        for(let game of games) {
            html += `<tr><td>${game.gameId}</td><td><button type="button" data-game-id="${game.gameId}" class="join-game-btn">Join game</button></td></tr>`
        }
        
        $('body').on('click', '.join-game-btn', (e) => {
           var gameId = $(e.target).data('game-id');
           this.joinGame(gameId);
        });
        table.append($(html));
    }

    appendToElement(elem) {
        elem.append(this.element);
    }
}