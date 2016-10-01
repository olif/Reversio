import {Board} from './board.js';
import {Signin} from './signin.js';
import {GamesTable} from './games-table.js';
import {GameServer} from './gameserver.js';
import $ from 'jquery';
import socketio from 'socketio';

export class App {
    constructor() {
        this.signin = new Signin((username) => this.onSignin(username));
        this.signin.appendToElement($('.container'));
        this.gameServer = new GameServer();
        this.gamesTable = new GamesTable();
    }

    onSignin(userName) {
        this.gameServer
            .signinPlayer(userName)
            .then((data) => {
                console.log('user signed in');
                this.bystander = data;
                this.showGamesTable();
            })
            .catch(() => console.log('felaa fel fel'));
    }

    showGamesTable() {
        this.gameServer.loadGames().then((data) => {
            $('.container').empty();
            this.gamesTable.updateTable(data);
            this.gamesTable.appendToElement($('.container'));
        })
    }
} 

let app = new App();
var websocket = new WebSocket('ws://localhost:53274');
websocket.onopen = function() {
    console.log('websocket open');
}

websocket.onmessage = function(evt) {
    console.log(evt);
}

$('.send').on('click', function() {
    websocket.send('testing');
});