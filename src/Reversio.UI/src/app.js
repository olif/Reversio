import {Board} from './board.js';
import {Signin} from './signin.js';
import {IndexPage} from './index.js';
import {GameServer} from './gameserver.js';
import {GamePage} from './game-page.js';
import $ from 'jquery';
import socketio from 'socketio';

export class App {
    constructor() {
        this.bystander = null;
        this.signin = new Signin((username) => this.onSignin(username));
        this.signin.appendToElement($('.container'));
        this.gameServer = new GameServer();
        this.gamesTable = new IndexPage(() => this.onCreateNewGame());
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

    onCreateNewGame() {
        this.gameServer
        .createNewGame()
        .then((gameState) => this.startGame(gameState));
    }

    startGame() {
        $('.container').empty();
        this.gamePage = new GamePage(this.gameServer);
        this.gamePage.appendToElement($('.container'));
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
