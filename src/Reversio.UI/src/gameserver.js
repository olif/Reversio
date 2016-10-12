import $ from 'jquery';

export class GameServer {
    constructor() {
        console.log('game server created');
        this.uri = 'http://localhost:53274/';
        this.bystander = null;
        this.socket = null;
        this.gamestate = null;  
    }

   signinPlayer(username) {
        let uri = this.uri;
        let socket = this.socket;
        let that = this;
        return new Promise(function(resolve, reject) {
            $.post(uri + 'api/game/signin', 
                {
                    name: username
                }, function(data) {
                    console.log(data);
                    this.bystander = data;
                    that.createConnection(data);
                    resolve(data);
                })
                .fail(reject);
        });
    }

    createConnection(bystander) {
        this.socket = new WebSocket('ws://localhost:53274');
        this.socket.onopen = (e) => console.log(`ws connection open ${e}`);
    }

    loadGames() {
        let uri = this.uri;
        return new Promise(function(resolve, reject) {
            $.get(uri + 'api/game/games/active', resolve).fail(reject);
        });
    }

    makeMove(pos) {
        this.socket.send(JSON.stringify(pos))
    }

    createNewGame() {
        let uri = this.uri;
        let that = this;
        return new Promise(function(resolve, reject) {
            $.post(uri + 'api/game', 
            that.bystander, function(response) {
                that.gamestate = response;
                resolve(response);
            }).fail(reject);
        });
    }
}