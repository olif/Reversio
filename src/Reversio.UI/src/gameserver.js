import $ from 'jquery';

export class GameServer {
    constructor() {
        console.log('game server created');
        this.uri = 'http://localhost:53274/';
    }

   signinPlayer(username) {
        let uri = this.uri;
        return new Promise(function(resolve, reject) {
            $.post(uri + 'api/game/signin', 
                {name: username}, 
                resolve)
                .fail(reject);
        });
    }

    loadGames() {
        let uri = this.uri;
        return new Promise(function(resolve, reject) {
            $.get(uri + 'api/game/games/active', resolve).fail(reject);
        });
    }
}