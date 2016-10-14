import $ from 'jquery';

export class GameServer {
    constructor() {
        console.log('game server created');
        this.uri = 'http://localhost:53274/';
        this.bystander = null;
        this.socket = null;
        this.gameState = null;  
        this.gameStateChangedListeners = [];
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
                    that.bystander = data;
                    that.createConnection(data);
                    resolve(data);
                })
                .fail(reject);
        });
    }
    
    addGameStateListener(listener) {
        this.gameStateChangedListeners.push((gameState) => listener(gameState));
    }
    
    onGameStateChanged(gameState) {
        for(let listener of this.gameStateChangedListeners) {
            listener(gameState);
        }
    }

    createConnection(bystander) {
        this.socket = new WebSocket('ws://localhost:53274');
        this.socket.onopen = (e) => console.log(`ws connection open ${e}`);
        this.socket.onmessage = (e) => {
            console.log('message received');
            console.log(e);
            let msg = JSON.parse(e.data);
            switch(msg.messageType) {
               case 'reversio.gameStateChanged':
                this.gameState = msg.payload;
                this.onGameStateChanged(this.gameState);
            }
        }
    }

    loadGames() {
        let uri = this.uri;
        return new Promise(function(resolve, reject) {
            $.get(uri + 'api/game/games/active', resolve).fail(reject);
        });
    }

    makeMove(pos) {
        let obj = {
            messageType: 'reversio.move',
            payload: {
                bystander: this.bystander,
                position: pos,
                gameId: this.gameState.gameId
            }
        };

        this.socket.send(JSON.stringify(obj))
    }

    createNewGame() {
        let uri = this.uri;
        let that = this;
        return new Promise(function(resolve, reject) {
            $.post(uri + 'api/game', 
            that.bystander, function(response) {
                that.gameState = response;
                resolve(response);
            }).fail(reject);
        });
    }
}