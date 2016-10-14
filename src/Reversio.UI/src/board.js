export class Board {
    constructor(gameState, onMoveCallback) {
        this.element = this.createElement(gameState);
        this.onMoveCallback = onMoveCallback;
    }

    createElement(gameState) {
        let html = `
            <table class="board ${gameState.discOfNextMove.color == -1 ? 'board-black-player' : 'board-white-player'}">

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
        move.addClass(this.getDiscClass(validMove.disc.color)).removeClass('disc-none');
        
        for(let flip of toFlip) {
            let query = `[data-pos-x="${flip.x}"][data-pos-y="${flip.y}"]`;
            var pos = this.element.find(query);
            setTimeout(function() {
                pos.toggleClass('disc-black disc-white');
            }, 200);
        }
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