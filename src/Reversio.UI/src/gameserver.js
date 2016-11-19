import $ from 'jquery';

export class GameServer {
    constructor(gameStartedCallback) {
        console.log('game server created');
        this.uri = 'http://localhost:53274/';
        this.bystander = null;
        this.socket = null;
        this.gameState = null;  
        this.gameStateChangedListeners = [];
        this.gameStartedCallback = (event) => gameStartedCallback(event);
    }

   signinPlayer(username) {
        let uri = this.uri;
        let socket = this.socket;
        let that = this;
        return new Promise(function(resolve, reject) {
            $.post(uri + 'api/games/signin', 
                {
                    name: username
                }, function(data) {
                    that.bystander = data;
                    that.createConnection();
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
    
    waitForOpponent() {
        console.log('waitForOpponent');
        let msg = {
            messageType: 'reversio.event.startGameWithRandomPlayer',
            payload: { 
                bystander: this.bystander
            }
        };
        this.socket.send(JSON.stringify(msg));
    }

    createConnection() {
        this.socket = new WebSocket(`ws://localhost:53274?sessionId=${this.bystander.id}`);
        this.socket.onopen = (e) => console.log(`ws connection open ${e}`);
        this.socket.onmessage = (e) => {
            console.log('message received');
            console.log(e);
            let msg = JSON.parse(e.data);
            switch (msg.messageType) {
                case 'reversio.event.gameStateChanged':
                    this.gameState = msg.payload.currentState;
                    this.onGameStateChanged(this.gameState);
                    break;

                case 'reversio.event.newGameCreated':
                    console.log('a new game was created');
                    break;

                case 'reversio.event.gameStarted':
                    this.gameState = msg.payload.currentState;
                    this.gameStartedCallback(msg.payload);
                    break;

            }
        }
    }

    loadGames() {
        let uri = this.uri;
        return new Promise(function(resolve, reject) {
            $.get(uri + 'api/games/active', resolve).fail(reject);
        });
    }

    makeMove(pos) {
        let obj = {
            messageType: 'reversio.event.move',
            payload: {
                bystander: this.bystander,
                position: pos,
                gameId: this.gameState.gameId
            }
        };

        this.socket.send(JSON.stringify(obj))
    }

    joinGame(gameId) {
        let uri = this.uri;
        let that = this;
        return new Promise(function(resolve, reject) {
            $.post(uri + `api/games/${gameId}/join`, that.bystander, function(response) {
                that.gameState = response;
                that.createConnection();
                resolve(response);
            }).fail(reject);
        });
    }
    
    createNewGame() {
        let uri = this.uri;
        let that = this;
        return new Promise(function(resolve, reject) {
            $.post(uri + 'api/games', 
            that.bystander, function(response) {
                that.gameState = response;
                that.createConnection();
                resolve(response);
            }).fail(reject);
        });
    }
}