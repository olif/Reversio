import {Board} from './board.js';
import {Signin} from './signin.js';
import {IndexPage} from './index.js';
import {GameServer} from './gameserver.js';
import {GamePage} from './game-page.js';
import $ from 'jquery';
require('./app.scss');
export class App {
    constructor() {
        this.bystander = null;
        this.signin = new Signin((username) => this.onSignin(username));
        this.signin.appendToElement($('.container'));
        this.gameServer = new GameServer(
           (event) => this.onGameCreated(event)
        );
        this.gamesTable = new IndexPage(
            () => this.onCreateNewGame(), 
            (gameId) => this.onJoinGame(gameId), 
            () => this.onCreateNewRandomGame());
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
    
    onGameCreated(gameCreatedEvent) {
        console.log('game was created....');
        console.log(this);
        this.startGame(gameCreatedEvent.playerAssignedDisc.color, gameCreatedEvent.currentState);
    }
    
    onCreateNewRandomGame() {
        console.log(this);
        console.log('waiting...')
        this.gameServer
        .waitForOpponent();
    }

    onCreateNewGame() {
        this.gameServer
        .createNewGame()
        .then((gameState) => this.startGame(-1, gameState));
    }
    
    onJoinGame(gameId) {
        this.gameServer
        .joinGame(gameId)
        .then((gameState) => this.startGame(1, gameState));
    }

    startGame(color) {
        $('.container').empty();
        this.gamePage = new GamePage(color, this.gameServer);
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
