export class Board {
    constructor(playerColor, gameState, onMoveCallback) {
        this.element = this.createElement(playerColor, gameState);
        this.onMoveCallback = onMoveCallback;
    }

    createElement(playerColor, gameState) {
        console.log(playerColor);
        let html = `
            <div class="game-container">
                <div class="standings-container">
                    <span id="black-player-standing">${gameState.blackPlayerStatus.name}: <span class="score">${gameState.blackPlayerStatus.score}</span></span>
                    <span id="white-player-standing">${gameState.whitePlayerStatus.name}: <span class="score">${gameState.whitePlayerStatus.score}</span></span>
                </div>
                <table class="board ${playerColor == -1 ? 'board-black-player' : 'board-white-player'}">

                ${gameState.currentState.reduce((rowAcc, row, i) => {
                    return rowAcc += 
                    `<tr>

                        ${row.reduce((colAcc, col, j) => {
                            return colAcc += 
                            `<td>
                                <span data-pos-y="${i}" data-pos-x="${j}" class="disc ${this.getDiscClass(col)}"></span>
                            </td>`
                        }, '')}

                        </tr>`
                }, '')}
                </table>
            </div>
        `;

        $('body').on('click', '.board', (event) => {
            let element = $(event.target);
            let x = element.data('pos-x');
            let y = element.data('pos-y');

            if(x !== undefined && y !== undefined) {
                this.onMoveCallback({x: x, y: y});
            }
        })

        let element = $(html);
        return $(html);
    }
    
    update(gameState) {
        let validMove = gameState.lastValidMove;
        let toFlip = gameState.discsFlipped;
        let query = `[data-pos-x="${validMove.position.x}"][data-pos-y="${validMove.position.y}"]`;
        let move = this.element.find(query);
        let count = 0;
        move.addClass(this.getDiscClass(validMove.disc.color)).removeClass('disc-none');
        
        for(let flip of toFlip) {
            count += 1;
            let query = `[data-pos-x="${flip.x}"][data-pos-y="${flip.y}"]`;
            let pos = this.element.find(query);
            setTimeout(function() {
                pos.toggleClass('disc-black disc-white');
            }, 100*count);
        }
        
        let blackPlayerScoreElem = this.element.find('#black-player-standing .score');
        let whitePlayerScoreElem = this.element.find('#white-player-standing .score');
        
        blackPlayerScoreElem.html(gameState.blackPlayerStatus.score);
        whitePlayerScoreElem.html(gameState.whitePlayerStatus.score);
    }

    getDiscClass(nr) {
        if(nr == -1) return 'disc-black';
        if(nr == 0) return 'disc-none';
        if(nr == 1) return 'disc-white';
    }

    appendToElement(elem) {
        console.log(elem);
        elem.append(this.element);
    }
}