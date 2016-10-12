export class Board {
    constructor(gamestate, onMoveCallback) {
        this.element = this.createElement(gamestate);
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